<template>
  <div>
    <el-card>
      <el-row :gutter="20" style="margin-top:10px;">
        <el-col :span="8">
          <div class="grid-content bg-purple">
            <el-card class="box-card">
              <div slot="header" class="clearfix">
                <span>个人主页</span>
              </div>
              <div class="name-role">
                <span class="sender">您好！{{ dataForm.userName }}</span>
              </div>
              <div class="registe-info">
                <span class="registe-info">

                </span>
              </div>
              <el-divider></el-divider>
              <div class="personal-relation">
                <div class="relation-item">您的职权:
                  <div style="float: right; padding-right:20px;">
                    <el-tag type="success" size="medium ">{{ dataForm.rolePermission === 9? "超级管理员":(dataForm.rolePermission === 8?"客服":"销售员")}}</el-tag>
                  </div>
                </div>
              </div>
              <div class="personal-relation">
                <div class="relation-item">您的编号:
                  <div style="float: right; padding-right:20px;">{{ dataForm.userID }}</div>
                </div>
              </div>
              <div class="personal-relation">
                <div class="relation-item">您的用户ID卡:
                  <div style="float: right; padding-right:20px;">{{ dataForm.userIDCard }}</div>
                </div>
              </div>
            </el-card>
          </div>
        </el-col>
        <el-col :span="16">
          <div class="grid-content bg-purple">
            <el-card class="box-card">
              <div slot="header" class="clearfix">
                <span>基本资料</span>
              </div>
              <div>
                <el-form label-width="80px" :model="dataForm" ref="editFormRef" :rules="editFormRules"  size="small" label-position="right">
                  <el-form-item label="姓名" prop="userName">
                    <el-input auto-complete="off" v-model="dataForm.userName" v-bind:disabled=!this.editButtonFun></el-input>
                  </el-form-item>
                  <el-form-item label="手机号" prop="userPhone">
                    <el-input auto-complete="off" v-model="dataForm.userPhone" v-bind:disabled=!this.editButtonFun></el-input>
                  </el-form-item>
                  <el-form-item label="邮箱" prop="userEmail">
                    <el-input maxlength="18" v-model="dataForm.userEmail" v-bind:disabled=!this.editButtonFun></el-input>
                  </el-form-item>
                  <el-form-item>
                    <el-button size="medium" type="primary" @click="editPasswordDialog(dataForm.userID)">修改密码
                    </el-button>
                    <el-button size="medium" type="warning" @click="editPersonData" v-if="editButtonFun===false">修改
                    </el-button>
                    <el-button size="medium" type="primary" v-if="editButtonFun===true" @click="postPersonData">提交
                    </el-button>
                    <el-button size="medium" type="primary" v-if="editButtonFun===true" @click="returnPersonData">取消
                    </el-button>
                  </el-form-item>
                </el-form>
              </div>
            </el-card>
          </div>
        </el-col>
      </el-row>
    </el-card>
    <el-dialog
      title="修改密码"
      :visible.sync="PasswordDialog"
      width="35%"
      @close="editPasswordDialogClosed">
      <el-form :model="dataForm" :rules="editFormRules" ref="editFormRef" label-width="120px">
<!--        <el-form-item label="您的初始密码">-->
<!--          <el-input v-model="dataForm.userPassword" disabled show-password></el-input>-->
<!--        </el-form-item>-->
        <el-form-item label="请输入新密码">
          <el-input v-model="dataForm.userPassword" show-password></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="PasswordDialog = false">取 消</el-button>
        <el-button type="primary" @click="editPasswordData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { UpdateUsersData, GetPersonData, GetUpdateData } from '@/api/UserManagement'

export default {
  data() {
    const checkuserName = (rule, value, callback) => {
      if (value === null || value.length < 3) {
        callback(new Error('用户名为空或少于3个字符！'))
      } else {
        callback()
      }
    }
    const checkuserPhone = (rule, value, callback) => {
      if (value.length !== 11) {
        callback(new Error('手机号码位数错误！'))
      } else {
        callback()
      }
    }
    const checkuserEmail = (rule, value, callback) => {
      var regEmail = /^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/
      if (value === null || !regEmail.test(value)) {
        callback(new Error('请输入正确的邮箱！'))
      } else {
        callback()
      }
    }
    const checkuserPassword = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入密码！'))
      } else {
        callback()
      }
    }
    return {
      editFormRules: {
        userName: [
          { required: true, message: '用户名为空或少于3个字符！', trigger: 'blur' },
          { validator: checkuserName, trigger: 'blur' }
        ],
        userPhone: [
          { required: true, message: '手机号码位数错误', trigger: 'blur' },
          { validator: checkuserPhone, trigger: 'blur' }
        ],
        userEmail: [
          { required: true, message: '请输入正确的邮箱！', trigger: 'blur' },
          { validator: checkuserEmail, trigger: 'blur' }
        ],
        userPassword: [
          { required: true, message: '请输入密码！', trigger: 'blur' },
          { validator: checkuserPassword, trigger: 'blur' }
        ]
      },
      dataForm: {},
      PasswordDialog: false,
      editButtonFun: false
    }
  },
  created() {
    this.getPersonData()
  },
  methods: {
    editPasswordData() {
      this.$refs.editFormRef.validate(valid => {
        if (!valid) return
        this.$confirm('成功修改密码将退出系统, 是否继续?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          UpdateUsersData({
            userID: this.dataForm.userID,
            userName: this.dataForm.userName,
            userPassword: this.dataForm.userPassword,
            userIDCard: this.dataForm.userIDCard,
            userPhone: this.dataForm.userPhone,
            userEmail: this.dataForm.userEmail,
            rolePermission: this.dataForm.rolePermission
          }).then(response => {
            if (response.code !== 200) {
              return this.$message.error('修改失败')
            }
            this.editButtonFun = false
            this.$message.success('修改成功')
            this.logout()
          })
        })
      })
    },
    async logout() {
      await this.$store.dispatch('user/logout')
      // this.$router.push(`/login?redirect=${this.$route.fullPath}`)
      this.$router.push(`/login?redirect=%2Fdashboard`)
    },
    editPasswordDialogClosed() {
      this.PasswordDialog = false
    },
    editPasswordDialog(id) {
      GetUpdateData({ userID: id }).then(response => {
        this.dataForm = response.data[0]
      })
      this.PasswordDialog = true
    },
    editPersonData() {
      this.editButtonFun = true
    },
    postPersonData() {
      this.$refs.editFormRef.validate(valid => {
        if (!valid) return
        UpdateUsersData({
          userID: this.dataForm.userID,
          userName: this.dataForm.userName,
          userPassword: this.dataForm.userPassword,
          userIDCard: this.dataForm.userIDCard,
          userPhone: this.dataForm.userPhone,
          userEmail: this.dataForm.userEmail,
          rolePermission: this.dataForm.rolePermission
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editButtonFun = false
          this.$message.success('更新成功')
        })
      })
    },
    returnPersonData() {
      this.$refs.editFormRef.resetFields()
      this.getPersonData()
      this.editButtonFun = false
    },
    async getPersonData() {
      GetPersonData().then(response => {
        this.dataForm = response.data[0]
      })
    }
  }
}
</script>

<style lang="scss" scoped>
//卡片样式
.text {
  font-size: 14px;
}

.item {
  margin-bottom: 18px;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both
}

.box-card {
  width: 100%;
}

//文本样式区
.name-role {
  font-size: 16px;
  padding: 5px;
  text-align: center;
}

.sender {
  text-align: center;
}

.registe-info {
  text-align: center;
  padding-top: 10px;
}

.personal-relation {
  font-size: 16px;
  padding: 0px 5px 15px;
  margin-right: 1px;
  width: 100%
}

.relation-item {
  padding: 12px;

}

.dialog-footer {
  padding-top: 10px;
  padding-left: 10%;
}

//布局样式区
.el-row {
  margin-bottom: 20px;

  &:last-child {
    margin-bottom: 0;
  }
}

.el-col {
  border-radius: 4px;
}

.bg-purple-dark {
  background: #99a9bf;
}

.bg-purple {
  background: #d3dce6;
}

.bg-purple-light {
  background: #e5e9f2;
}

.grid-content {
  border-radius: 4px;
  min-height: 36px;
}

.row-bg {
  padding: 10px 0;
  background-color: #f9fafc;
}
</style>
