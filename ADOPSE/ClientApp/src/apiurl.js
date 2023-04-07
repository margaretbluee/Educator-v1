export async function GetApiUrl() {
    // Make an HTTP GET request to the "/api/env" endpoint on the server
    console.log("test")
    const response = await fetch('http://localhost:5000/api/env');
    console.log("respons" + response);
    const env = await response.json();
  
    // Extract the API URL from the environment variables
    const apiUrl = env.REACT_APP_API_URL;
  
    // Return the API URL with a trailing slash
    console.log("api:" + apiUrl);
    return apiUrl ? apiUrl + '/' : '';

}