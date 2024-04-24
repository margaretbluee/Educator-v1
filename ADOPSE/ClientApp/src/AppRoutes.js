// import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Login from "./components/login";
import Register from "./components/register";
import ModulePage from "./components/modulepage/";
import MyModules from "./components/myModules";
//import GoogleCalendar from "./components/googleCalendar";
import ModuleInfo from "./components/moduleInfo";
import Lecturer from "./components/Lecturer";
import LecturersPage from "./components/LecturerPage";

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
    path: "/lecturers",
    element: <LecturersPage />,
  },
  {
    path: "/myLearning",
    element: <MyModules />,
  },
  {
    path: "/module",
    element: <ModuleInfo />,
  },
  {
    path: "/lecturer",
    element: <Lecturer />,
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/register",
    element: <Register />,
  },
];

export default AppRoutes;
