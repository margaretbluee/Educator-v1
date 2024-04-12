import React from "react";

function GoogleCalendar(props) {
 
    async function getAllEvents() {
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

        let eventList = await fetch("/api/calendar/getAllEventsFromGoogle", requestOptions)
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

                // if(error.response.status === 401){
                //     return error;
                // }
            });

          sendEvents(eventList)  // send events to database in order to be stored


        console.log(eventList);        

        alert("Synchronized as well as posible! Thanks for you services.");

    }

    async function sendEvents(events) {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        myHeaders.append(
            "Authorization",
            `Bearer ${localStorage.getItem("token")}`
        );

        var raw = JSON.stringify(events);

        var requestOptions = {
            method: "POST",
            headers: myHeaders,
            body: raw,
            redirect: "follow",
        };

        fetch("/api/calendar/addModules", requestOptions)
            .then((response) =>{
              if(!response.ok)  {
                throw new Error("'addModules' could not fetch recource")
              }
              response.text()
            }) 
            .then((result) => console.log(result))
            .catch((error) => console.log("error", error));
    }


  return (
    <div>
  {/*    {session ? (*/}
  {/*      <>*/}
  {/*        <h2>Hey there {session.user.email}</h2>*/}
  {/*        <button onClick={() => signOut()}>Sign Out</button>*/}
  {/*        <button onClick={() => retrieveAllEvents()}>Retrieve</button>*/}
  {/*      </>*/}
  {/*    ) : (*/}
  {/*      <>*/}
  {/*        <button onClick={() => googleSignIn()}>Empa mesa</button>*/}
  {/*      </>*/}
  {/*        )} <br></br><br></br>*/}
  {/*    </div>*/}
          <>
              <button onClick={() => getAllEvents()}> RetrieveTest</button>
          </>
      </div>    
  );
}

export default GoogleCalendar;
