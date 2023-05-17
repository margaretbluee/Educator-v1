import React from "react";
import {
  useSession,
  useSupabaseClient,
  // useSessionContext,
} from "@supabase/auth-helpers-react";

function GoogleCalendar(props) {
  const session = useSession();
  const supabase = useSupabaseClient();

  async function googleSignIn() {
    const { error } = await supabase.auth.signInWithOAuth({
      provider: "google",
      options: {
        scopes: "https://www.googleapis.com/auth/calendar",
      },
    });
    if (error) {
      alert("EEERRROOORRR");
    }
  }

  async function signOut() {
    await supabase.auth.signOut();
  }

  async function retrieveEventsFromCalendar(calendarId) {
    let toReturn;
    await fetch(
      `https://www.googleapis.com/calendar/v3/calendars/${calendarId}/events`,
      {
        method: "GET",
        headers: {
          Authorization: "Bearer " + session.provider_token, // Access token for google
        },
      }
    )
      .then((data) => {
        return data.json();
      })
      .then((data) => {
        //console.log(data.items);
        toReturn = data.items;
        // return data.items;
        // alert("Event created, check your Google Calendar!");
      });

    return toReturn;
  }
  
  async function sendEvents(events) {

    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("Authorization",`Bearer ${localStorage.getItem("token")}`)

    var raw = JSON.stringify(events);

    var requestOptions = {
      method: 'POST',
      headers: myHeaders,
      body: raw,
      redirect: 'follow'
    };

    fetch("https://localhost:44442/api/calendar/addModules", requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
  }

  async function retrieveAllEvents() {
    var calendarsId = [];
    var events = [];
    await fetch(
      "https://www.googleapis.com/calendar/v3/users/me/calendarList",
      {
        method: "GET",
        headers: {
          Authorization: "Bearer " + session.provider_token, // Access token for google
        },
      }
    )
      .then((data) => {
        return data.json();
      })
      .then((data) => {
        // console.log(data.items);

        data.items.forEach((calendar) => calendarsId.push(calendar.id));
        // console.log(calendarsId);
        alert("Calendar List retrieved");
      });

    //console.log("finish");

    for (let i = 1; i < calendarsId.length; i++) {
      events.push(await retrieveEventsFromCalendar(calendarsId[i]));
    }
    
    console.log(events);
    
    let array2d = [];
    events.forEach((calendar) => {
      calendar.forEach((event) => {
        //console.log(event.organizer.email);
        let eventArray = [event.organizer.email,event.summary,event.description || "",event.start.dateTime,event.end.dateTime];
        array2d.push(eventArray);
      })
    });
    
    console.log(array2d);
    
    await sendEvents(array2d);
    
    alert("send");
    
  }

  return (
    <div>
      {session ? (
        <>
          <h2>Hey there {session.user.email}</h2>
          <button onClick={() => signOut()}>Sign Out</button>
          <button onClick={() => retrieveAllEvents()}>Retrieve</button>
        </>
      ) : (
        <>
          <button onClick={() => googleSignIn()}>Empa mesa</button>
        </>
      )}
    </div>
  );
}

export default GoogleCalendar;
