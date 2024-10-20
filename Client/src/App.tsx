import { Suspense } from "react";
import { useRoutes } from "react-router";
import routes from "~react-pages";

function App() {
  return (
    <Suspense>
      {" "}
      <div className="w-full h-full absolute items-center justify-center flex flex-col">
        {useRoutes(routes)}
      </div>
    </Suspense>
  );
}

export default App;
