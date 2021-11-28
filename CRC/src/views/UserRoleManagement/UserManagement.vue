<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item clearable label="用户编号" label-width='120px'>
          <el-input v-model="req.contactName" placeholder="请输入用户编号"></el-input>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="getUsers">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddUsersDialog">添加用户</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <el-form >
        <el-form-item style='margin-left: 35px'>
          <el-button @click="resetDateFilter" type="danger" icon="el-icon-delete">批量删除</el-button>
        </el-form-item>
      </el-form >
      <!--      表格-->
      <el-table
        ref="filterTable"
        :data="UsersList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          type="selection"
          width="55">
        </el-table-column>
        <el-table-column
          fixed
          sortable
          prop="userID"
          label="用户编号"
          width="150">
        </el-table-column>
        <el-table-column
          sortable
          prop="userName"
          label="用户姓名"
          width="150">
        </el-table-column>
        <el-table-column
          prop="userIDCard"
          label="用户id卡"
          width="150">
        </el-table-column>
        <el-table-column
          sortable
          prop="userPhone"
          label="用户电话"
          width="150">
        </el-table-column>
        <el-table-column
          prop="userEmail"
          label="用户邮箱"
          width="250">
        </el-table-column>
        <el-table-column
          sortable
          prop="rolePermission"
          label="权限等级"
          width="110"
        >
          <template slot-scope="scope">
            {{ scope.row.rolePermission == 9?'超级管理员':(scope.row.rolePermission==8?'客服':'普通销售员')}}
          </template>
        </el-table-column>
        <el-table-column
          fixed="right"
          label="操作"
          width="200">
          <template slot-scope="scope">
            <!--修改-->
            <el-button type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.userID)">
            </el-button>
            <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeleteUser(scope.row.userID)"></el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        style='padding-top: 10px'
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="current"
        :page-sizes="[7, 10, 20, 30]"
        :page-size="size"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total">
      </el-pagination>
    </el-card>
    <el-dialog
      title="增加信息"
      :visible.sync="addDialogVisible"
      width="35%"
      @close="addDialogClosed">
      <el-form :model="addForm" :rules="addFormRules" ref="addFormRef" label-width="120px">
        <el-form-item label="用户姓名" prop="userName">
          <el-input v-model="addForm.userName" placeholder="用户姓名"></el-input>
        </el-form-item>
        <el-form-item label="用户密码" prop="userPassword">
          <el-input v-model="addForm.userPassword" placeholder="用户密码"></el-input>
        </el-form-item>
        <el-form-item label="用户id卡" prop="userIDCard">
          <el-input v-model="addForm.userIDCard" placeholder="用户id卡"></el-input>
        </el-form-item>
        <el-form-item label="用户电话" prop="userPhone">
          <el-input v-model="addForm.userPhone" placeholder="用户电话"></el-input>
        </el-form-item>
        <el-form-item label="用户邮箱" prop="userEmail">
          <el-input v-model="addForm.userEmail" placeholder="用户邮箱"></el-input>
        </el-form-item>
        <el-form-item label="权限等级" prop="rolePermission">
          <el-select v-model="addForm.rolePermission" placeholder="请选择角色">
            <el-option label="超级管理员" value="9"></el-option>
            <el-option label="客服人员" value="8"></el-option>
            <el-option label="普通销售员" value="7"></el-option>
          </el-select>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addUserData">确 定</el-button>
      </span>
    </el-dialog>
    <el-dialog
      title="修改信息"
      :visible.sync="editDialogVisible"
      width="35%"
      @close="editDialogClosed">
      <el-form :model="editForm" :rules="editFormRules" ref="editFormRef" label-width="120px">
        <el-form-item label="用户编号">
          <el-input v-model="editForm.userID" disabled></el-input>
        </el-form-item>
        <el-form-item label="用户姓名" prop="userName">
          <el-input v-model="editForm.userName"></el-input>
        </el-form-item>
        <el-form-item label="用户密码" prop="userPassword">
          <el-input v-model="editForm.userPassword"></el-input>
        </el-form-item>
        <el-form-item label="用户id卡" prop="userIDCard">
          <el-input v-model="editForm.userIDCard"></el-input>
        </el-form-item>
        <el-form-item label="用户电话" prop="userPhone">
          <el-input v-model="editForm.userPhone"></el-input>
        </el-form-item>
        <el-form-item label="用户邮箱" prop="userEmail">
          <el-input v-model="editForm.userEmail"></el-input>
        </el-form-item>
        <el-form-item label="权限等级" prop="rolePermission">
          <el-input v-model="editForm.rolePermission"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editUsersData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// import { findPetList } from '@/api/pets'
// import { findCategoryAndCount } from '@/api/pets'
import { GetUsersData, InsertUsersData, UpdateUsersData, RemoveUsersData, GetUpdateData } from '@/api/UserManagement'

export default {
  name: 'UsersPerformanceManagement',
  data() {
    const checkuserName = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入用户名'))
      } else {
        callback()
      }
    }
    const checkuserPassword = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入用户密码'))
      } else {
        callback()
      }
    }
    const checkuserIDCard = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入用户ID卡'))
      } else {
        callback()
      }
    }
    const checkuserPhone = (rule, value, callback) => {
      if (value.length !== 11) {
        callback(new Error('请输入电话号码'))
      } else {
        callback()
      }
    }
    const checkuserEmail = (rule, value, callback) => {
      var regEmail = /^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/
      if (value === null || !regEmail.test(value)) {
        callback(new Error('请输入正确的邮箱'))
      } else {
        callback()
      }
    }
    const checkrolePermission = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入用户权限'))
      } else {
        callback()
      }
    }
    return {
      req: {
        userID: '',
        userName: '',
        userPassword: '',
        userIDCard: '',
        userPhone: '',
        userEmail: '',
        rolePermission: ''
      },
      addFormRules: {
        userName: [
          { required: true, message: '输入用户姓名', trigger: 'blur' },
          { validator: checkuserName, trigger: 'blur' }
        ],
        userPassword: [
          { required: true, message: '输入用户密码', trigger: 'blur' },
          { validator: checkuserPassword, trigger: 'blur' }
        ],
        userIDCard: [
          { required: true, message: '请输入用户ID卡', trigger: 'blur' },
          { validator: checkuserIDCard, trigger: 'blur' }
        ],
        userPhone: [
          { required: true, message: '请输入用户电话', trigger: 'blur' },
          { validator: checkuserPhone, trigger: 'blur' }
        ],
        userEmail: [
          { required: true, message: '请输入用户电话', trigger: 'blur' },
          { validator: checkuserEmail, trigger: 'blur' }
        ],
        rolePermission: [
          { required: true, message: '请输入电子邮箱', trigger: 'blur' },
          { validator: checkrolePermission, trigger: 'blur' }
        ]
      },
      editFormRules: {
        userName: [
          { required: true, message: '输入用户姓名', trigger: 'blur' },
          { validator: checkuserName, trigger: 'blur' }
        ],
        userPassword: [
          { required: true, message: '输入用户密码', trigger: 'blur' },
          { validator: checkuserPassword, trigger: 'blur' }
        ],
        userIDCard: [
          { required: true, message: '请输入用户ID卡', trigger: 'blur' },
          { validator: checkuserIDCard, trigger: 'blur' }
        ],
        userPhone: [
          { required: true, message: '请输入用户电话', trigger: 'blur' },
          { validator: checkuserPhone, trigger: 'blur' }
        ],
        userEmail: [
          { required: true, message: '请输入用户电话', trigger: 'blur' },
          { validator: checkuserEmail, trigger: 'blur' }
        ],
        rolePermission: [
          { required: true, message: '请输入电子邮箱', trigger: 'blur' },
          { validator: checkrolePermission, trigger: 'blur' }
        ]
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      UsersList: [],
      editForm: {},
      addForm: {},
      size: 7,
      total: 10,
      current: 1,
      currentPage4: 4,
      petsCount: [],
      // 控制弹窗的显示与隐藏
      addOrUpdateVisible: false
    }
  },
  created() {
    this.getUsers()
  },
  methods: {
    resetDateFilter() {
      this.$refs.filterTable.clearFilter('projectEnd')
    },
    clearFilter() {
      this.$refs.filterTable.clearFilter()
    },
    filterHandler(value, row, column) {
      const property = column['property']
      return row[property] === value
    },
    onSubmit() {
      console.log('submit!')
    },
    // 当每页条数改变
    handleSizeChange(val) {
      this.size = val
      this.getPetList()
    },
    // 页码改变
    handleCurrentChange(val) {
      this.current = val
      this.getSalesman()
    },
    editDialogClosed() {
      this.$refs.editFormRef.resetFields()
    },
    addDialogClosed() {
      this.$refs.addFormRef.resetFields()
    },
    async getUsers() {
      GetUsersData().then(response => {
        this.UsersList = response.data
        console.log('rolePermission: ' + this.UsersList[0].rolePermission)
      })
    },
    editUsersData() {
      this.$refs.editFormRef.validate(valid => {
        if (!valid) return
        // 发起请求
        UpdateUsersData({
          userID: this.editForm.userID,
          userName: this.editForm.userName,
          userPassword: this.editForm.userPassword,
          userIDCard: this.editForm.userIDCard,
          userPhone: this.editForm.userPhone,
          userEmail: this.editForm.userEmail,
          rolePermission: this.editForm.rolePermission
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editDialogVisible = false
          this.getUsers()
          this.$message.success('更新成功')
        })
      })
    },
    addUserData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertUsersData({
          userName: this.addForm.userName,
          userPassword: this.addForm.userPassword,
          userIDCard: this.addForm.userIDCard,
          userPhone: this.addForm.userPhone,
          userEmail: this.addForm.userEmail,
          rolePermission: this.addForm.rolePermission
        }).then(response => {
          if (response.code !== 200) {
            if (response.code === 400) return this.$message.error('您没有权限！')
            else return this.$message.error('添加失败')
          } else {
            this.addDialogVisible = false
            this.getUsers()
            this.$message.success('添加成功')
          }
        })
      })
    },
    async showAddUsersDialog() {
      this.addDialogVisible = true
    },
    async showEditDialog(id) {
      GetUpdateData({ userID: id }).then(response => {
        this.editForm = response.data[0]
      })
      this.editDialogVisible = true
    },
    DeleteUser(id) {
      this.$confirm('此操作将删除该用户信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => { // 确定执行then方法)
        RemoveUsersData({ userID: id }).then(response => {
          if (response.code === 200) {
            this.$message({
              type: 'success',
              message: '删除成功!'
            })
            this.getUsers()
          }
        })
      }
      )
    },
    chongzhi() {
      this.req.userID = ''
      this.req.userName = ''
      this.req.userPassword = ''
      this.req.userIDCard = ''
      this.req.userPhone = ''
      this.req.userEmail = ''
      this.req.rolePermission = ''
      this.getUsersList()
    },
    // 按钮点击事件 显示弹窗组件
    show() {
      this.addOrUpdateVisible = true
    },
    previewImg(url) {
      this.$hevueImgPreview(url)
    }
  }
}
</script>

<style scoped>

</style>
