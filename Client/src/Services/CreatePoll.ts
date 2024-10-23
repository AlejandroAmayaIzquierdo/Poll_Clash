import { VITE_APP_BASE_URL } from "@/constants";

const CreatePoll = async (question: string, options: string[]) => {
  const resp = await fetch(`${VITE_APP_BASE_URL}/polls`, {
    method: "POST",
    body: JSON.stringify({ text: question, options }),
    headers: {
      "Content-Type": "application/json",
    },
  });
  return (await resp.json()) as API.StandarResponse<API.Poll>;
};

export { CreatePoll };
