// interface BasePageProps {}
import { Button } from "@/components/ui/button";

const BasePage: React.FC = () => {
  return (
    <div className="w-full min-h-screen bg-gradient-to-b from-gray-100 to-gray-200 flex flex-col items-center justify-center p-4">
      <div className="max-w-3xl mx-auto text-center space-y-8">
        <h1 className="text-4xl font-bold tracking-tight text-gray-900 sm:text-6xl">
          Create and Share Polls Easily
        </h1>
        <p className="text-xl text-gray-600 leading-8">
          Welcome to our Poll Creator! This platform allows you to quickly
          create custom polls and gather opinions on any topic. Whether you're
          making decisions with friends, collecting feedback for work, or just
          curious about what others think, our tool makes it simple and fun.
        </p>
        <ul className="text-left text-gray-600 space-y-2">
          <li className="flex items-center">
            <svg
              className="h-6 w-6 mr-2 text-green-500"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M5 13l4 4L19 7"
              />
            </svg>
            Create polls with multiple options
          </li>
          <li className="flex items-center">
            <svg
              className="h-6 w-6 mr-2 text-green-500"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M5 13l4 4L19 7"
              />
            </svg>
            Customize your questions and answers
          </li>
          <li className="flex items-center">
            <svg
              className="h-6 w-6 mr-2 text-green-500"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M5 13l4 4L19 7"
              />
            </svg>
            Share polls easily with friends or colleagues
          </li>
          <li className="flex items-center">
            <svg
              className="h-6 w-6 mr-2 text-green-500"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M5 13l4 4L19 7"
              />
            </svg>
            View results in real-time
          </li>
        </ul>
        <div>
          <a href="/create">
            <Button size="lg" className="text-lg px-8 py-6">
              Create Your First Poll
            </Button>
          </a>
        </div>
      </div>
    </div>
  );
};

export default BasePage;
