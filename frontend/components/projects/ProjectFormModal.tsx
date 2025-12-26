'use client';

import React, { useState, useEffect } from 'react';
import Modal from '../ui/Modal';
import Input from '../ui/Input';
import Button from '../ui/Button';
import { Project, ProjectStatus, Priority, CreateProjectDto, UpdateProjectDto } from '@/lib/types/project.types';
import { projectsApi } from '@/lib/api/projects';

interface ProjectFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  project?: Project;
}

const ProjectFormModal: React.FC<ProjectFormModalProps> = ({ isOpen, onClose, onSuccess, project }) => {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    status: ProjectStatus.PLANNING,
    priority: Priority.MEDIUM,
    startDate: '',
    endDate: '',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (isOpen) {
      if (project) {
        setFormData({
          name: project.name,
          description: project.description || '',
          status: project.status,
          priority: project.priority,
          startDate: project.startDate ? project.startDate.split('T')[0] : '',
          endDate: project.endDate ? project.endDate.split('T')[0] : '',
        });
      } else {
        setFormData({
          name: '',
          description: '',
          status: ProjectStatus.PLANNING,
          priority: Priority.MEDIUM,
          startDate: '',
          endDate: '',
        });
      }
      setError('');
    }
  }, [isOpen, project]);

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
      const projectData: any = {
        name: formData.name,
        description: formData.description || null,
        status: formData.status,
        priority: formData.priority,
        startDate: formData.startDate ? `${formData.startDate}T00:00:00Z` : null,
        endDate: formData.endDate ? `${formData.endDate}T00:00:00Z` : null,
      };

      if (project) {
        await projectsApi.updateProject(project.id, projectData as UpdateProjectDto);
      } else {
        await projectsApi.createProject(projectData as CreateProjectDto);
      }

      onSuccess();
      onClose();
    } catch (err: any) {
      console.error('Failed to save project:', err);
      console.error('Error response:', err.response?.data);
      const errorMsg = err.response?.data?.message ||
                       err.response?.data?.errors ||
                       err.response?.data?.title ||
                       'Failed to save project';
      setError(typeof errorMsg === 'object' ? JSON.stringify(errorMsg) : errorMsg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={project ? 'Edit Project' : 'Create Project'} size="lg">
      <form onSubmit={handleSubmit} className="space-y-4">
        {error && (
          <div className="p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg text-sm">
            {error}
          </div>
        )}

        <Input
          type="text"
          name="name"
          label="Project Name"
          value={formData.name}
          onChange={handleChange}
          required
          placeholder="Enter project name"
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
            placeholder="Enter project description"
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
              <option value={ProjectStatus.PLANNING}>Planning</option>
              <option value={ProjectStatus.ACTIVE}>Active</option>
              <option value={ProjectStatus.ON_HOLD}>On Hold</option>
              <option value={ProjectStatus.COMPLETED}>Completed</option>
              <option value={ProjectStatus.ARCHIVED}>Archived</option>
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
              <option value={Priority.LOW}>Low</option>
              <option value={Priority.MEDIUM}>Medium</option>
              <option value={Priority.HIGH}>High</option>
              <option value={Priority.URGENT}>Urgent</option>
            </select>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <Input
            type="date"
            name="startDate"
            label="Start Date"
            value={formData.startDate}
            onChange={handleChange}
            disabled={loading}
          />

          <Input
            type="date"
            name="endDate"
            label="End Date"
            value={formData.endDate}
            onChange={handleChange}
            disabled={loading}
          />
        </div>

        <div className="flex justify-end gap-3 pt-4">
          <Button type="button" variant="secondary" onClick={onClose} disabled={loading}>
            Cancel
          </Button>
          <Button type="submit" variant="primary" isLoading={loading}>
            {project ? 'Update Project' : 'Create Project'}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default ProjectFormModal;
