import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [{
      path: 'dashboard',
      name: 'Dashboard',
      component: () => import('@/views/dashboard/index'),
      meta: { title: '首页', icon: 'dashboard' }
    }]
  },
  {
    path: '/Myself',
    component: Layout,
    redirect: '/Myself',
    children: [{
      path: '/Myself',
      name: 'Myself',
      component: () => import('@/views/Myself/Myself'),
      meta: { title: '个人中心', icon: 'dashboard' }
    }]
  },
  {
    path: '/404',
    component: () => import('@/views/404'),
    hidden: true
  }
]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router

export const asyncRouterMap = [
  // 客户管理
  {
    path: '/CustomerManagement',
    component: Layout,
    redirect: '/CustomerManagement/CustomerContract',
    name: 'CustomerManagement',
    meta: {
      title: '客户管理',
      icon: 'el-icon-s-help',
      roles: [9, 7]
    },
    children: [
      {
        path: '/CustomerContract',
        name: 'CustomerContract',
        component: () => import('@/views/CustomerManagement/CustomerContract/index'),
        meta: {
          title: '客户合约信息',
          icon: 'table',
          roles: [9, 7]
        },
        children: [
          {
            path: '/ContractManagement',
            name: 'ContractManagement',
            component: () => import('@/views/CustomerManagement/CustomerContract/ContractManagement'),
            meta: {
              title: '合约管理',
              icon: 'tree',
              roles: [9, 7]
            }
          },
          {
            path: '/SignerInformationManagement',
            name: 'SignerInformationManagement',
            component: () => import('@/views/CustomerManagement/CustomerContract/SignerInformationManagement'),
            meta: { title: '签单人信息管理',
              icon: 'tree',
              roles: [9, 7]
            }
          }
        ]
      },
      {
        path: '/CustomerContactManagement',
        name: 'CustomerContactManagement',
        component: () => import('@/views/CustomerManagement/CustomerContactManagement'),
        meta: {
          title: '客户联系人管理',
          icon: 'tree',
          roles: [9, 7]
        }
      },
      {
        path: '/CustomerMessageManagement',
        name: 'CustomerMessageManagement',
        component: () => import('@/views/CustomerManagement/CustomerMessageManagerment'),
        meta: {
          title: '客户信息管理',
          icon: 'tree',
          roles: [9, 7]
        }
      }
    ]
  },
  // 客户关系管理
  {
    path: '/CustomerRelationshipManagement',
    component: Layout,
    redirect: '/CustomerRelationshipManagement/ActionManagement',
    name: 'CustomerRelationshipManagement',
    meta: {
      title: '客户关系管理',
      icon: 'el-icon-s-help',
      roles: [9, 7, 8]
    },
    children: [
      {
        path: '/ActionManagement',
        redirect: '/ActionManagement',
        name: 'ActionManagement',
        component: () => import('@/views/CustomerRelationshipManagement/ActionManagerment/index'),
        meta: {
          title: '行动管理',
          icon: 'table',
          roles: [9, 7]
        },
        children: [
          {
            path: '/ActionPlanManagement',
            name: 'ActionPlanManagement',
            component: () => import('@/views/CustomerRelationshipManagement/ActionManagerment/ActionPlanManagement'),
            meta: {
              title: '行动计划管理',
              icon: 'tree',
              roles: [9, 7]
            }
          },
          {
            path: '/Record',
            name: 'Record',
            component: () => import('@/views/CustomerRelationshipManagement/ActionManagerment/Record'),
            meta: {
              title: '记录',
              icon: 'tree',
              roles: [9, 7]
            }
          }
        ]
      },
      {
        path: '/ComplainAndSuggestionManagement',
        name: 'ComplainAndSuggestionManagement',
        component: () => import('@/views/CustomerRelationshipManagement/ComplaintAndSuggestionManagement'),
        meta: {
          title: '投诉建议管理',
          icon: 'tree',
          roles: [8, 9, 7]
        }
      }
    ]
  },
  // 销售管理
  {
    path: '/SaleManagement',
    component: Layout,
    meta: {
      title: '销售员管理',
      icon: 'form',
      roles: [9]
    },
    children: [
      {
        path: '/SaleManagement/SalesManagement',
        name: 'SalesManagement',
        component: () => import('@/views/SalesManagement/SalesManagement'),
        meta: {
          title: '销售员管理',
          icon: 'form',
          roles: [9]
        }
      },
      {
        path: '/SalesPerformanceManagement',
        name: 'SalesPerformanceManagement',
        component: () => import('@/views/SalesManagement/SalesPerformanceManagement'),
        meta: {
          title: '销售员业绩管理',
          icon: 'form',
          roles: [9]
        }
      },
      {
        path: '/SalesTeamManagement',
        name: 'SalesTeamManagement',
        component: () => import('@/views/SalesManagement/SalesTeamManagement'),
        meta: {
          title: '销售员分组管理',
          icon: 'form',
          roles: [9]
        }
      }
    ]
  },
  // 用户角色管理
  {
    path: '/UserRoleManagement',
    component: Layout,
    meta: {
      title: '用户角色管理',
      icon: 'form',
      roles: [9]
    },
    children: [
      {
        path: '/RoleManagement',
        name: 'RoleManagement',
        component: () => import('@/views/UserRoleManagement/RoleManagement'),
        meta: {
          title: '角色管理',
          icon: 'form',
          roles: [9]
        }
      },
      {
        path: '/UserManagement',
        name: 'UserManagement',
        component: () => import('@/views/UserRoleManagement/UserManagement'),
        meta: {
          title: '用户管理',
          icon: 'form',
          roles: [9]
        }
      }
    ]
  },
  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]
