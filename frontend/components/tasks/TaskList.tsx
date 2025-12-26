'use client';

import React from 'react';
import TaskCard from './TaskCard';
import { Task, TaskStatus } from '@/lib/types/task.types';

interface TaskListProps {
  tasks: Task[];
  isLoading?: boolean;
  onEdit?: (task: Task) => void;
  onDelete?: (taskId: number) => void;
  onStatusChange?: (taskId: number, status: TaskStatus) => void;
}

const TaskList: React.FC<TaskListProps> = ({
  tasks,
  isLoading = false,
  onEdit,
  onDelete,
  onStatusChange,
}) => {
  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  // Ensure tasks is an array
  const taskList = Array.isArray(tasks) ? tasks : [];

  if (taskList.length === 0) {
    return (
      <div className="text-center py-16">
        <div className="text-6xl mb-4">📝</div>
        <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
          No tasks yet
        </h3>
        <p className="text-gray-600 dark:text-gray-400">
          Create your first task to get started
        </p>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {taskList.map((task) => (
        <TaskCard
          key={task.id}
          task={task}
          onEdit={onEdit}
          onDelete={onDelete}
          onStatusChange={onStatusChange}
        />
      ))}
    </div>
  );
};

export default TaskList;
