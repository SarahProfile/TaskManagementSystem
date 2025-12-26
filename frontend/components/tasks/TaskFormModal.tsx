'use client';

import React, { useState, useEffect } from 'react';
import Modal from '../ui/Modal';
import Input from '../ui/Input';
import Button from '../ui/Button';
import { Task, TaskStatus, TaskPriority, CreateTaskDto, UpdateTaskDto } from '@/lib/types/task.types';
import { tasksApi } from '@/lib/api/tasks';
import { projectsApi } from '@/lib/api/projects';

interface TaskFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  task?: Task;
}

const TaskFormModal: React.FC<TaskFormModalProps> = ({ isOpen, onClose, onSuccess, task }) => {
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    status: TaskStatus.TODO,
    priority: TaskPriority.MEDIUM,
    dueDate: '',
    projectId: '',
    assignedToId: '',
  });
  const [projects, setProjects] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (isOpen) {
      fetchProjects();
      if (task) {
        setFormData({
          title: task.title,
          description: task.description || '',
          status: task.status,
          priority: task.priority,
          dueDate: task.dueDate ? task.dueDate.split('T')[0] : '',
          projectId: task.projectId?.toString() || '',
          assignedToId: task.assignedToId?.toString() || '',
        });
      } else {
        setFormData({
          title: '',
          description: '',
          status: TaskStatus.TODO,
          priority: TaskPriority.MEDIUM,
          dueDate: '',
          projectId: '',
          assignedToId: '',
        });
      }
      setError('');
    }
  }, [isOpen, task]);

  const fetchProjects = async () => {
    try {
      const response = await projectsApi.getAllProjects();
      setProjects(response.items || []);
    } catch (err) {
      console.error('Failed to fetch projects:', err);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const taskData: any = {
        title: formData.title,
        description: formData.description || null,
        status: formData.status,
        priority: formData.priority,
        dueDate: formData.dueDate ? `${formData.dueDate}T00:00:00Z` : null,
        projectId: formData.projectId && formData.projectId.trim() !== '' ? formData.projectId : null,
        assignedToId: formData.assignedToId && formData.assignedToId.trim() !== '' ? formData.assignedToId : null,
      };

      if (task) {
        await tasksApi.updateTask(task.id, taskData as UpdateTaskDto);
      } else {
        await tasksApi.createTask(taskData as CreateTaskDto);
      }

      onSuccess();
      onClose();
    } catch (err: any) {
      console.error('Failed to save task:', err);
      console.error('Error response:', err.response?.data);
      const errorMsg = err.response?.data?.message ||
                       err.response?.data?.errors ||
                       err.response?.data?.title ||
                       'Failed to save task';
      setError(typeof errorMsg === 'object' ? JSON.stringify(errorMsg) : errorMsg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={task ? 'Edit Task' : 'Create Task'} size="lg">
      <form onSubmit={handleSubmit} className="space-y-4">
        {error && (
          <div className="p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg text-sm">
            {error}
          </div>
        )}

        <Input
          type="text"
          name="title"
          label="Title"
          value={formData.title}
          onChange={handleChange}
          required
          placeholder="Enter task title"
          disabled={loading}
        />

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Description
          </label>
          <textarea
            name="description"
            value={formData.description}
            onChange={handleChange}
            rows={3}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
            placeholder="Enter task description"
            disabled={loading}
          />
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Status
            </label>
            <select
              name="status"
              value={formData.status}
              onChange={handleChange}
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
              disabled={loading}
            >
              <option value={TaskStatus.TODO}>To Do</option>
              <option value={TaskStatus.IN_PROGRESS}>In Progress</option>
              <option value={TaskStatus.IN_REVIEW}>In Review</option>
              <option value={TaskStatus.DONE}>Done</option>
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Priority
            </label>
            <select
              name="priority"
              value={formData.priority}
              onChange={handleChange}
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
              disabled={loading}
            >
              <option value={TaskPriority.LOW}>Low</option>
              <option value={TaskPriority.MEDIUM}>Medium</option>
              <option value={TaskPriority.HIGH}>High</option>
              <option value={TaskPriority.URGENT}>Urgent</option>
            </select>
          </div>
        </div>

        <Input
          type="date"
          name="dueDate"
          label="Due Date"
          value={formData.dueDate}
          onChange={handleChange}
          disabled={loading}
        />

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Project (Optional)
          </label>
          <select
            name="projectId"
            value={formData.projectId}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
            disabled={loading}
          >
            <option value="">No Project</option>
            {projects.map((project) => (
              <option key={project.id} value={project.id}>
                {project.name}
              </option>
            ))}
          </select>
        </div>

        <div className="flex justify-end gap-3 pt-4">
          <Button type="button" variant="secondary" onClick={onClose} disabled={loading}>
            Cancel
          </Button>
          <Button type="submit" variant="primary" isLoading={loading}>
            {task ? 'Update Task' : 'Create Task'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default TaskFormModal;
