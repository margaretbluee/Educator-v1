import React from 'react';
import { useSession, useSupabaseClient, useSessionContext } from '@supabase/auth-helpers-react';

function GoogleCalendar(props) {
    const session = useSession();
    const supabase = useSupabaseClient();
    
    async function googleSignIn()
    {
        const {error} = await  supabase.auth.signInWithOAuth({
            provider : "google",
            options : {
                scopes : 'https://www.googleapis.com/auth/calendar'
            }
        });
        if(error)
        {
            alert("EEERRROOORRR");
        }
    }

    async function signOut() {
        await supabase.auth.signOut();
    }
    
    async function retrieveData()
    {
        await fetch("https://www.googleapis.com/calendar/v3/calendars/f7330b12b3c55cba4e3ef9ad8de3dc0bbf583a4d4f9d9ca7821f56b8fbc67647%40group.calendar.google.com/events", {
            method: "GET",
            headers: {
                'Authorization':'Bearer ' + session.provider_token // Access token for google
            }
        }).then((data) => {
            return data.json();
        }).then((data) => {
            console.log(data);
            alert("Event created, check your Google Calendar!");
        });
    }

    
    return (
        <div>
            {
                session ? 
                    <>
                        <h2>Hey there {session.user.email}</h2>
                        <button onClick={() => signOut()}>Sign Out</button>
                        <button onClick={() => retrieveData()}>Retrieve</button>  
                    </>
                    :
                    <>
                        <button onClick={() => googleSignIn()}>Empa mesa</button>
                    </>
            }
        </div>
    );
}

export default GoogleCalendar;