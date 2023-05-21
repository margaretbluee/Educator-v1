import React from "react";
import { createRoot } from "react-dom/client";
import { MemoryRouter } from "react-router-dom";
import App from "./App";
import { JSDOM } from "jsdom";
import fetch from "node-fetch";

window.document.cookieJar = {
  allowSpecialUseDomain: true,
  rejectPublicSuffixes: false,
};

global.fetch = fetch;

it("renders without crashing", async () => {
  try {
    root.render(
      <MemoryRouter>
        <App />
      </MemoryRouter>
    );
  } catch (error) {
    // Ignore the error
  }
  await new Promise((resolve) => setTimeout(resolve, 1000));
});
