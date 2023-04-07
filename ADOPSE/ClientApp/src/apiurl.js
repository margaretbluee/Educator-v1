export async function GetApiUrl() {
    try {
      // Make an HTTP GET request to the "/api/env" endpoint on the server
      const response = await fetch('/api/env');
      const env = await response.json();
  
      // Extract the API URL from the environment variables
      const apiUrl = env.REACT_APP_API_URL;
  
      // Return the API URL with a trailing slash
      return apiUrl ? apiUrl + '/' : '';
    } catch (error) {
      // Log the error (optional)
  
      // Return an empty string if there was an error fetching the environment variables
      return '';
    }
  }