/// <reference types="vite/client" />

declare namespace APP {}

declare namespace API {
  interface WebSocketEvent {
    eventType: string;
    [key: string]: unknown;
  }

  interface HelloEvent extends WebSocketEvent {
    name: string;
  }
}
