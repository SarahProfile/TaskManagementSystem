'use client';

import React, { useEffect, useState } from 'react';
import { useAuth } from '@/lib/hooks/useAuth';
import Card, { CardHeader, CardBody } from '@/components/ui/Card';
import { tasksApi } from '@/lib/api/tasks';
import { projectsApi } from '@/lib/api/projects';
import { TaskStatus, TaskPriority } from '@/lib/types/task.types';

interface Stats {
  totalTasks: number;
  totalProjects: number;
  tasksByStatus: Record<TaskStatus, number>;
  tasksByPriority: Record<TaskPriority, number>;
}

export default function DashboardPage() {
  const { user } = useAuth();
  const [stats, setStats] = useState<Stats>({
    totalTasks: 0,
    totalProjects: 0,
    tasksByStatus: {
      [TaskStatus.TODO]: 0,
      [TaskStatus.IN_PROGRESS]: 0,
      [TaskStatus.IN_REVIEW]: 0,
      [TaskStatus.DONE]: 0,
    },
    tasksByPriority: {
      [TaskPriority.LOW]: 0,
      [TaskPriority.MEDIUM]: 0,
      [TaskPriority.HIGH]: 0,
      [TaskPriority.URGENT]: 0,
    },
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const [tasksResponse, projectsResponse] = await Promise.all([
          tasksApi.getAllTasks(),
          projectsApi.getAllProjects(),
        ]);

        const tasksByStatus = tasksResponse.items.reduce((acc, task) => {
          acc[task.status] = (acc[task.status] || 0) + 1;
          return acc;
        }, {} as Record<TaskStatus, number>);

        const tasksByPriority = tasksResponse.items.reduce((acc, task) => {
          acc[task.priority] = (acc[task.priority] || 0) + 1;
          return acc;
        }, {} as Record<TaskPriority, number>);

        setStats({
          totalTasks: tasksResponse.totalCount,
          totalProjects: projectsResponse.totalCount,
          tasksByStatus: {
            [TaskStatus.TODO]: tasksByStatus[TaskStatus.TODO] || 0,
            [TaskStatus.IN_PROGRESS]: tasksByStatus[TaskStatus.IN_PROGRESS] || 0,
            [TaskStatus.IN_REVIEW]: tasksByStatus[TaskStatus.IN_REVIEW] || 0,
            [TaskStatus.DONE]: tasksByStatus[TaskStatus.DONE] || 0,
          },
          tasksByPriority: {
            [TaskPriority.LOW]: tasksByPriority[TaskPriority.LOW] || 0,
            [TaskPriority.MEDIUM]: tasksByPriority[TaskPriority.MEDIUM] || 0,
            [TaskPriority.HIGH]: tasksByPriority[TaskPriority.HIGH] || 0,
            [TaskPriority.URGENT]: tasksByPriority[TaskPriority.URGENT] || 0,
          },
        });
      } catch (error) {
        console.error('Error fetching stats:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchStats();
  }, []);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="px-4 sm:px-6 lg:px-8 py-6">
      <div className="mb-10">
        <h1 className="text-5xl font-bold bg-gradient-to-r from-blue-600 via-purple-600 to-pink-600 bg-clip-text text-transparent mb-2">
          Welcome back, {user?.username}!
        </h1>
        <p className="text-gray-600 dark:text-gray-400 text-lg">Here's what's happening with your tasks today</p>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-10">
        <Card variant="bordered" className="bg-gradient-to-br from-blue-500 to-blue-600 text-white border-none shadow-xl hover:shadow-2xl hover:scale-105 transition-all duration-300">
          <CardBody>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm font-medium text-blue-100 mb-1">Total Tasks</p>
                <p className="text-4xl font-bold">{stats.totalTasks}</p>
              </div>
              <div className="bg-white/20 p-3 rounded-full">
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
                </svg>
              </div>
            </div>
          </CardBody>
        </Card>

        <Card variant="bordered" className="bg-gradient-to-br from-green-500 to-emerald-600 text-white border-none shadow-xl hover:shadow-2xl hover:scale-105 transition-all duration-300">
          <CardBody>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm font-medium text-green-100 mb-1">Total Projects</p>
                <p className="text-4xl font-bold">{stats.totalProjects}</p>
              </div>
              <div className="bg-white/20 p-3 rounded-full">
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                </svg>
              </div>
            </div>
          </CardBody>
        </Card>

        <Card variant="bordered" className="bg-gradient-to-br from-yellow-500 to-orange-500 text-white border-none shadow-xl hover:shadow-2xl hover:scale-105 transition-all duration-300">
          <CardBody>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm font-medium text-yellow-100 mb-1">In Progress</p>
                <p className="text-4xl font-bold">
                  {stats.tasksByStatus[TaskStatus.IN_PROGRESS]}
                </p>
              </div>
              <div className="bg-white/20 p-3 rounded-full">
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 10V3L4 14h7v7l9-11h-7z" />
                </svg>
              </div>
            </div>
          </CardBody>
        </Card>

        <Card variant="bordered" className="bg-gradient-to-br from-purple-500 to-pink-600 text-white border-none shadow-xl hover:shadow-2xl hover:scale-105 transition-all duration-300">
          <CardBody>
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm font-medium text-purple-100 mb-1">Completed</p>
                <p className="text-4xl font-bold">
                  {stats.tasksByStatus[TaskStatus.DONE]}
                </p>
              </div>
              <div className="bg-white/20 p-3 rounded-full">
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
            </div>
          </CardBody>
        </Card>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card variant="bordered" className="shadow-lg hover:shadow-xl transition-shadow duration-300 bg-gradient-to-br from-white to-blue-50 dark:from-gray-800 dark:to-gray-900">
          <CardHeader>
            <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
              <svg className="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
              </svg>
              Tasks by Status
            </h2>
          </CardHeader>
          <CardBody>
            <div className="space-y-3">
              {Object.entries(stats.tasksByStatus).map(([status, count]) => (
                <div key={status} className="flex items-center justify-between p-3 rounded-lg bg-white dark:bg-gray-800 shadow-sm hover:shadow-md transition-shadow">
                  <span className="font-medium text-gray-700 dark:text-gray-300 flex items-center gap-2">
                    <span className="w-3 h-3 rounded-full bg-gradient-to-r from-blue-500 to-purple-500"></span>
                    {status.replace('_', ' ')}
                  </span>
                  <span className="font-bold text-xl text-gray-900 dark:text-white bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">{count}</span>
                </div>
              ))}
            </div>
          </CardBody>
        </Card>

        <Card variant="bordered" className="shadow-lg hover:shadow-xl transition-shadow duration-300 bg-gradient-to-br from-white to-purple-50 dark:from-gray-800 dark:to-gray-900">
          <CardHeader>
            <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
              <svg className="w-6 h-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              Tasks by Priority
            </h2>
          </CardHeader>
          <CardBody>
            <div className="space-y-3">
              {Object.entries(stats.tasksByPriority).map(([priority, count]) => (
                <div key={priority} className="flex items-center justify-between p-3 rounded-lg bg-white dark:bg-gray-800 shadow-sm hover:shadow-md transition-shadow">
                  <span className="font-medium text-gray-700 dark:text-gray-300 flex items-center gap-2">
                    <span className="w-3 h-3 rounded-full bg-gradient-to-r from-purple-500 to-pink-500"></span>
                    {priority}
                  </span>
                  <span className="font-bold text-xl text-gray-900 dark:text-white bg-gradient-to-r from-purple-600 to-pink-600 bg-clip-text text-transparent">{count}</span>
                </div>
              ))}
            </div>
          </CardBody>
        </Card>
      </div>
    </div>
  );
}
