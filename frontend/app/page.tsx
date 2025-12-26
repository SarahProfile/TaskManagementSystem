import Link from 'next/link';
import Button from '@/components/ui/Button';

export default function Home() {
  return (
    <main className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-100 dark:from-gray-900 dark:to-gray-800">
      <div className="text-center px-4">
        <h1 className="text-6xl font-bold text-gray-900 dark:text-white mb-4">
          Task Management System
        </h1>
        <p className="text-xl text-gray-600 dark:text-gray-300 mb-8 max-w-2xl mx-auto">
          Organize your projects, track your tasks, and boost your productivity with our comprehensive task management solution.
        </p>

        <div className="flex gap-4 justify-center items-center flex-wrap">
          <Link href="/login">
            <Button variant="primary" size="lg">
              Sign In
            </Button>
          </Link>
          <Link href="/register">
            <Button variant="outline" size="lg">
              Create Account
            </Button>
          </Link>
        </div>

        <div className="mt-16 grid grid-cols-1 md:grid-cols-3 gap-8 max-w-4xl mx-auto">
          <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-md">
            <div className="text-4xl mb-3">✓</div>
            <h3 className="text-lg font-semibold mb-2 text-gray-900 dark:text-white">Task Tracking</h3>
            <p className="text-gray-600 dark:text-gray-400">Efficiently manage and track all your tasks in one place</p>
          </div>
          <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-md">
            <div className="text-4xl mb-3">📁</div>
            <h3 className="text-lg font-semibold mb-2 text-gray-900 dark:text-white">Project Management</h3>
            <p className="text-gray-600 dark:text-gray-400">Organize tasks into projects and monitor progress</p>
          </div>
          <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-md">
            <div className="text-4xl mb-3">👥</div>
            <h3 className="text-lg font-semibold mb-2 text-gray-900 dark:text-white">Team Collaboration</h3>
            <p className="text-gray-600 dark:text-gray-400">Assign tasks and collaborate with your team members</p>
          </div>
        </div>
      </div>
    </main>
  );
}
