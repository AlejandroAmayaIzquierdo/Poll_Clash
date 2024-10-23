/// <reference types="vite/client" />
/// <reference types="vite-plugin-pages/client-react" />

declare namespace APP {
  interface Poll {
    Text: string;
    Options: Option[];
  }

  interface Option {
    OptionId: number;
    Text: string;
    Votes: number;
    PollId: string;
  }
}

declare namespace API {
  interface Poll {
    pollId: string;
    text: string;
    createdAt: string;
    options: Options[];
  }
  interface Options {
    optionId: number;
    text: string;
    votes: number;
    pollId: string;
  }
  interface StandarResponse<T> {
    Error: string;
    Detail: T;
  }
  interface WebSocketEvent {
    eventType: string;
    [key: string]: unknown;
  }

  interface HelloEvent extends WebSocketEvent {
    name: string;
  }
}
