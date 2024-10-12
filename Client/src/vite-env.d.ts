/// <reference types="vite/client" />

declare namespace APP {
  interface Vote {
    Text: string;
    Options: Option[];
  }

  interface Option {
    Id: number;
    Text: string;
    Votes: number;
  }
}

declare namespace API {
  interface WebSocketEvent {
    eventType: string;
    [key: string]: unknown;
  }

  interface HelloEvent extends WebSocketEvent {
    name: string;
  }
}
