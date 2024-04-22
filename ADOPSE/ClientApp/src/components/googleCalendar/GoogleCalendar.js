import React from "react";

function GoogleCalendar(props) {    

    async function FetchAllEventsFromGoogleAndSync() {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        myHeaders.append(
            "Authorization", `Bearer ${localStorage.getItem("token")}`
        );

        var requestOptions = {
            method: "GET",
            headers: myHeaders,
            redirect: "follow",
        };    
        
        let eventList = await fetch("/api/calendar/fetchAndSync", requestOptions)
            .then(response => {                
                if (!response.ok) {
                    throw new Error("Could not fetch resource")
                }
                return response.json()                
            })
            .then(data => {                
                console.log(data)
                return data;                          
            })
            .catch((error) => {
                console.log("error", error)
            });

        console.log(eventList);        

        alert("Synchronized as well as posible! Thanks for your services.");

    }

  return (
    <div>
          <>
              <button onClick={() => FetchAllEventsFromGoogleAndSync()}> Fetch And Sync</button>
          </>
      </div>    
  );
}

export default GoogleCalendar;
