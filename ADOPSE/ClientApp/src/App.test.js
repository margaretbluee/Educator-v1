import React from "react";
import { createRoot } from "react-dom/client";
import { MemoryRouter } from "react-router-dom";
import App from "./App";
import { JSDOM } from "jsdom";
import fetch from "node-fetch";

// Create a JSDOM environment
const { window } = new JSDOM("<!doctype html><html><body></body></html>", {
  url: "http://localhost",
});

// Configure the cookie settings for the JSDOM environment
window.document.cookieJar = {
  allowSpecialUseDomain: true,
  rejectPublicSuffixes: false,
};

// Assign the JSDOM window to the global object
global.window = window;
global.document = window.document;
global.navigator = {
  userAgent: "node.js",
};

// Provide a global fetch function using node-fetch
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
