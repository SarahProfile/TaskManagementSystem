'use client';

import React, { useEffect, useState } from 'react';
import { tasksApi } from '@/lib/api/tasks';
import TaskList from '@/components/tasks/TaskList';
import TaskFormModal from '@/components/tasks/TaskFormModal';
import Button from '@/components/ui/Button';
import { Task, TaskStatus } from '@/lib/types/task.types';

export default function TasksPage() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedTask, setSelectedTask] = useState<Task | undefined>();

  const fetchTasks = async () => {
    try {
      setLoading(true);
      setError('');
      const paginatedResult = await tasksApi.getAllTasks();
      // paginatedResult.items is the array of tasks
      setTasks(paginatedResult.items || []);
    } catch (err: any) {
      console.error('Failed to fetch tasks:', err);
      setError(err.response?.data?.message || 'Failed to fetch tasks');
      setTasks([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, []);

  const handleDelete = async (taskId: string) => {
    if (!confirm('Are you sure you want to delete this task?')) {
      return;
    }

    try {
      await tasksApi.deleteTask(taskId);
      setTasks(tasks.filter((task) => task.id !== taskId));
    } catch (err: any) {
      alert(err.response?.data?.message || 'Failed to delete task');
    }
  };

  const handleEdit = (task: Task) => {
    setSelectedTask(task);
    setIsModalOpen(true);
  };

  const handleCreate = () => {
    setSelectedTask(undefined);
    setIsModalOpen(true);
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedTask(undefined);
  };

  const handleModalSuccess = () => {
    fetchTasks();
  };

  const handleStatusChange = async (taskId: string, status: TaskStatus) => {
    try {
      const updatedTask = await tasksApi.updateTask(taskId, { status });
      setTasks(tasks.map((task) => (task.id === taskId ? updatedTask : task)));
    } catch (err: any) {
      alert(err.response?.data?.message || 'Failed to update task status');
    }
  };

  return (
    <div className="px-4 sm:px-6 lg:px-8 py-6">
      <div className="flex flex-col sm:flex-row sm:justify-between sm:items-center gap-4 mb-8">
        <div>
          <h1 className="text-4xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
            Tasks
          </h1>
          <p className="text-gray-600 dark:text-gray-400 mt-1">Manage and track your tasks</p>
        </div>
        <Button variant="primary" onClick={handleCreate} className="shadow-lg hover:shadow-xl transition-shadow">
          <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
          </svg>
          Create Task
        </Button>
      </div>

      {error && (
        <div className="mb-4 p-4 bg-red-100 border border-red-400 text-red-700 rounded-lg">
          {error}
        </div>
      )}

      <TaskList
        tasks={tasks}
        isLoading={loading}
        onEdit={handleEdit}
        onDelete={handleDelete}
        onStatusChange={handleStatusChange}
      />

      <TaskFormModal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        onSuccess={handleModalSuccess}
        task={selectedTask}
      />
    </div>
  );
}
