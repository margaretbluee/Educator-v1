// import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Login from "./components/login";
import Register from "./components/register";
import ModulePage from "./components/modulepage/";
import MyLearning from "./components/myLearning";
import GoogleCalendar from "./components/googleCalendar";

const AppRoutes = [
  {
    index: true,
    element: <Home />,
  },
  {
    path: "/modules",
    element: <ModulePage />,
  },
  {
    path: "/fetch-data",
    element: <MyLearning />,
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/register",
    element: <Register />,
  },
  {
    path: "/googleCalendar",
    element: <GoogleCalendar />,
  },
];

export default AppRoutes;
