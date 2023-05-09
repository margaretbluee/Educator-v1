// import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Login from "./components/login";
import Register from "./components/register";
import ModulePage from "./components/modulepage/";
import MyLearning from "./components/myLearning";

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
];

export default AppRoutes;
