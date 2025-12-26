'use client';

import React, { useEffect, useState } from 'react';
import { projectsApi } from '@/lib/api/projects';
import ProjectFormModal from '@/components/projects/ProjectFormModal';
import Card, { CardBody } from '@/components/ui/Card';
import Button from '@/components/ui/Button';
import { Project, ProjectStatus, Priority } from '@/lib/types/project.types';

export default function ProjectsPage() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedProject, setSelectedProject] = useState<Project | undefined>();

  const fetchProjects = async () => {
    try {
      setLoading(true);
      setError('');
      const paginatedResult = await projectsApi.getAllProjects();
      // paginatedResult.items is the array of projects
      setProjects(paginatedResult.items || []);
    } catch (err: any) {
      console.error('Failed to fetch projects:', err);
      setError(err.response?.data?.message || 'Failed to fetch projects');
      setProjects([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProjects();
  }, []);

  const handleDelete = async (projectId: string) => {
    if (!confirm('Are you sure you want to delete this project?')) {
      return;
    }

    try {
      await projectsApi.deleteProject(projectId);
      setProjects(projects.filter((project) => project.id !== projectId));
    } catch (err: any) {
      alert(err.response?.data?.message || 'Failed to delete project');
    }
  };

  const handleEdit = (project: Project) => {
    setSelectedProject(project);
    setIsModalOpen(true);
  };

  const handleCreate = () => {
    setSelectedProject(undefined);
    setIsModalOpen(true);
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setSelectedProject(undefined);
  };

  const handleModalSuccess = () => {
    fetchProjects();
  };

  const getStatusBadgeColor = (status: ProjectStatus) => {
    switch (status) {
      case ProjectStatus.PLANNING:
        return 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300';
      case ProjectStatus.ACTIVE:
        return 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300';
      case ProjectStatus.ON_HOLD:
        return 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300';
      case ProjectStatus.COMPLETED:
        return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300';
      case ProjectStatus.ARCHIVED:
        return 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const getPriorityBadgeColor = (priority: Priority) => {
    switch (priority) {
      case Priority.LOW:
        return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300';
      case Priority.MEDIUM:
        return 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300';
      case Priority.HIGH:
        return 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-300';
      case Priority.URGENT:
        return 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="px-4 sm:px-6 lg:px-8 py-6">
      <div className="flex flex-col sm:flex-row sm:justify-between sm:items-center gap-4 mb-8">
        <div>
          <h1 className="text-4xl font-bold bg-gradient-to-r from-purple-600 to-pink-600 bg-clip-text text-transparent">
            Projects
          </h1>
          <p className="text-gray-600 dark:text-gray-400 mt-1">Organize and manage your projects</p>
        </div>
        <Button variant="primary" onClick={handleCreate} className="shadow-lg hover:shadow-xl transition-shadow">
          <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
          </svg>
          Create Project
        </Button>
      </div>

      {error && (
        <div className="mb-4 p-4 bg-red-100 border border-red-400 text-red-700 rounded-lg">
          {error}
        </div>
      )}

      {projects.length === 0 ? (
        <div className="text-center py-16">
          <div className="text-6xl mb-4">📁</div>
          <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
            No projects yet
          </h3>
          <p className="text-gray-600 dark:text-gray-400">
            Create your first project to get started
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {projects.map((project) => (
            <Card key={project.id} variant="bordered" className="hover:shadow-xl hover:scale-[1.02] transition-all duration-300 bg-gradient-to-br from-white to-purple-50 dark:from-gray-800 dark:to-gray-900 border-l-4 border-l-purple-500">
              <CardBody>
                <div className="flex flex-col sm:flex-row sm:justify-between sm:items-start gap-2 mb-3">
                  <h3 className="text-lg font-bold text-gray-900 dark:text-white line-clamp-1 flex-1">
                    {project.name}
                  </h3>
                  <div className="flex gap-2 flex-wrap">
                    <span className={`px-3 py-1 text-xs font-semibold rounded-full shadow-sm ${getStatusBadgeColor(project.status)}`}>
                      {project.status.replace('_', ' ')}
                    </span>
                    <span className={`px-3 py-1 text-xs font-semibold rounded-full shadow-sm ${getPriorityBadgeColor(project.priority)}`}>
                      {project.priority}
                    </span>
                  </div>
                </div>

                {project.description && (
                  <p className="text-sm text-gray-600 dark:text-gray-400 mb-4 line-clamp-2">
                    {project.description}
                  </p>
                )}

                <div className="space-y-2 mb-4 text-sm">
                  {project.owner && (
                    <div className="flex items-center gap-2 bg-indigo-50 dark:bg-indigo-900/20 px-3 py-2 rounded-lg">
                      <svg className="w-4 h-4 text-indigo-600 dark:text-indigo-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                      </svg>
                      <span className="font-semibold text-indigo-900 dark:text-indigo-200">{project.owner.username}</span>
                    </div>
                  )}
                  {project.tasks && (
                    <div className="flex items-center gap-2 bg-green-50 dark:bg-green-900/20 px-3 py-2 rounded-lg">
                      <svg className="w-4 h-4 text-green-600 dark:text-green-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4" />
                      </svg>
                      <span className="font-medium text-green-900 dark:text-green-200">{project.tasks.length} Tasks</span>
                    </div>
                  )}
                </div>

                <div className="flex gap-2 pt-3 border-t border-gray-200 dark:border-gray-700">
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => handleEdit(project)}
                    className="flex-1 hover:bg-purple-50 dark:hover:bg-purple-900/20 transition-colors"
                  >
                    <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                    </svg>
                    Edit
                  </Button>
                  <Button
                    variant="danger"
                    size="sm"
                    onClick={() => handleDelete(project.id)}
                    className="flex-1"
                  >
                    <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                    </svg>
                    Delete
                  </Button>
                </div>
              </CardBody>
            </Card>
          ))}
        </div>
      )}

      <ProjectFormModal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        onSuccess={handleModalSuccess}
        project={selectedProject}
      />
    </div>
  );
}
