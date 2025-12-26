'use client';

import React from 'react';
import Card, { CardBody } from '@/components/ui/Card';
import Button from '@/components/ui/Button';
import { Task, TaskStatus, TaskPriority } from '@/lib/types/task.types';

interface TaskCardProps {
  task: Task;
  onEdit?: (task: Task) => void;
  onDelete?: (taskId: number) => void;
  onStatusChange?: (taskId: number, status: TaskStatus) => void;
}

const TaskCard: React.FC<TaskCardProps> = ({ task, onEdit, onDelete, onStatusChange }) => {
  const getStatusBadgeColor = (status: TaskStatus) => {
    switch (status) {
      case TaskStatus.TODO:
        return 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300';
      case TaskStatus.IN_PROGRESS:
        return 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300';
      case TaskStatus.IN_REVIEW:
        return 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300';
      case TaskStatus.DONE:
        return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const getPriorityBadgeColor = (priority: TaskPriority) => {
    switch (priority) {
      case TaskPriority.LOW:
        return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300';
      case TaskPriority.MEDIUM:
        return 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300';
      case TaskPriority.HIGH:
        return 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-300';
      case TaskPriority.URGENT:
        return 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const formatDate = (dateString?: string) => {
    if (!dateString) return null;
    return new Date(dateString).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
    });
  };

  return (
    <Card variant="bordered" className="hover:shadow-xl hover:scale-[1.02] transition-all duration-300 bg-gradient-to-br from-white to-gray-50 dark:from-gray-800 dark:to-gray-900 border-l-4 border-l-blue-500">
      <CardBody>
        <div className="flex flex-col sm:flex-row sm:justify-between sm:items-start gap-2 mb-3">
          <h3 className="text-lg font-bold text-gray-900 dark:text-white line-clamp-1 flex-1">
            {task.title}
          </h3>
          <div className="flex gap-2 flex-wrap">
            <span className={`px-3 py-1 text-xs font-semibold rounded-full shadow-sm ${getStatusBadgeColor(task.status)}`}>
              {task.status.replace('_', ' ')}
            </span>
            <span className={`px-3 py-1 text-xs font-semibold rounded-full shadow-sm ${getPriorityBadgeColor(task.priority)}`}>
              {task.priority}
            </span>
          </div>
        </div>

        {task.description && (
          <p className="text-sm text-gray-600 dark:text-gray-400 mb-4 line-clamp-2">
            {task.description}
          </p>
        )}

        <div className="space-y-2 mb-4 text-sm">
          {task.project && (
            <div className="flex items-center gap-2 bg-blue-50 dark:bg-blue-900/20 px-3 py-2 rounded-lg">
              <svg className="w-4 h-4 text-blue-600 dark:text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
              </svg>
              <span className="font-semibold text-blue-900 dark:text-blue-200">{task.project.name}</span>
            </div>
          )}
          {task.assignedTo && (
            <div className="flex items-center gap-2 bg-purple-50 dark:bg-purple-900/20 px-3 py-2 rounded-lg">
              <svg className="w-4 h-4 text-purple-600 dark:text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
              </svg>
              <span className="font-medium text-purple-900 dark:text-purple-200">{task.assignedTo.username}</span>
            </div>
          )}
          {task.dueDate && (
            <div className="flex items-center gap-2 bg-orange-50 dark:bg-orange-900/20 px-3 py-2 rounded-lg">
              <svg className="w-4 h-4 text-orange-600 dark:text-orange-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
              </svg>
              <span className="font-medium text-orange-900 dark:text-orange-200">{formatDate(task.dueDate)}</span>
            </div>
          )}
        </div>

        <div className="flex gap-2 pt-3 border-t border-gray-200 dark:border-gray-700">
          {onEdit && (
            <Button
              variant="outline"
              size="sm"
              onClick={() => onEdit(task)}
              className="flex-1 hover:bg-blue-50 dark:hover:bg-blue-900/20 transition-colors"
            >
              <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
              </svg>
              Edit
            </Button>
          )}
          {onDelete && (
            <Button
              variant="danger"
              size="sm"
              onClick={() => onDelete(task.id)}
              className="flex-1"
            >
              <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
              </svg>
              Delete
            </Button>
          )}
        </div>
      </CardBody>
    </Card>
  );
};

export default TaskCard;
