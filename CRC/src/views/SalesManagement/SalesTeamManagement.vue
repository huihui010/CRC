<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item label="组号" label-width='70px'>
          <el-select clearable v-model="req.groupID" placeholder="请选择组号" >
            <el-option
              v-for="item in petsCount"
              :key="item.petCategory"
              :label="item.petCategory"
              :value="item.petCategory">
              <span style="float: left">{{ item.petCategory }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">
                <span class='el-tag el-tag--success el-tag--mini el-tag--plain'> 共{{ item.categoryCount }}条记录</span>
              </span>
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item clearable label="组名" label-width='90px'>
          <el-input v-model="req.groupName" placeholder="请输入组名"></el-input>
        </el-form-item>
        <el-form-item clearable label="组长名" label-width='90px'>
          <el-input v-model="req.leaderName" placeholder="组长名"></el-input>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="GetGroupData">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddGroupDialog">添加</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <!--      表格-->
      <el-table
        :data="SelesGroupList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          prop="groupID"
          label="组号"
          width="120">
        </el-table-column>
        <el-table-column
          prop="groupName"
          label="组名"
          width="120">
        </el-table-column>
        <el-table-column
          prop="leaderName"
          label="组长"
          width="100">
        </el-table-column>
        <el-table-column
          label="操作"
          width="300">
          <template slot-scope='scope'>
            <!--修改-->
            <el-button type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.groupID)"></el-button>
            <!--删除-->
            <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeleteSalesGroup(scope.row.groupID)"></el-button>
            <el-button type="warning" size="mini" icon="el-icon-s-tools"></el-button>
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
      <el-form :model="addForm" :rules="addFormRules" ref="addFormRef" label-width="100px">
        <el-form-item label="组号" prop="groupID">
          <el-input v-model="addForm.groupID"></el-input>
        </el-form-item>
        <el-form-item label="组名" prop="groupName">
          <el-input v-model="addForm.groupName"></el-input>
        </el-form-item>
        <el-form-item label="组长名" prop="leaderName">
          <el-input v-model="addForm.leaderName"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addSalesGroupData">确 定</el-button>
      </span>
    </el-dialog>
    <el-dialog
      title="修改信息"
      :visible.sync="editDialogVisible"
      width="35%"
      @close="editDialogClosed">
      <el-form :model="editForm" :rules="editFormRules" ref="editFormRef" label-width="100px">
        <el-form-item label="组号">
          <el-input v-model="editForm.groupID"></el-input>
        </el-form-item>
        <el-form-item label="组名">
          <el-input v-model="editForm.groupName"></el-input>
        </el-form-item>
        <el-form-item label="组长姓名" prop="salesmanPhone">
          <el-input v-model="editForm.leaderName"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editSalesGroupData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// import { findPetList } from '@/api/pets'
// import { findCategoryAndCount } from '@/api/pets'
import { GetGroupData, InsertGroupData, UpdateGroupData, RemoveGroupData, GetUpdateData } from '@/api/SalesGroup'

export default {
  name: 'SalesManagement',
  data() {
    const checkgroupID = (rule, value, callback) => {
      if (value.length === null) {
        callback(new Error('请输入组号'))
      } else {
        callback()
      }
    }
    const checkgroupName = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入组名'))
      } else {
        callback()
      }
    }
    const checkleaderName = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入组长名'))
      } else {
        callback()
      }
    }
    return {
      req: {
        groupID: '',
        groupName: '',
        leaderName: ''
      },
      addFormRules: {
        groupID: [
          { required: true, message: '请输入组号', trigger: 'blur' },
          { validator: checkgroupID, trigger: 'blur' }
        ],
        groupName: [
          { required: true, message: '请输入组名', trigger: 'blur' },
          { validator: checkgroupName, trigger: 'blur' }
        ],
        leaderName: [
          { required: true, message: '请输入组长名', trigger: 'blur' },
          { validator: checkleaderName, trigger: 'blur' }
        ]
      },
      editFormRules: {
        leaderName: [
          { required: true, message: '请输入组长名', trigger: 'blur' },
          { validator: checkleaderName, trigger: 'blur' }
        ]
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      SelesGroupList: [],
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
    this.GetGroupData()
    // this.getCategoryAndCount()
  },
  methods: {
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
    async GetGroupData() {
      GetGroupData().then(response => {
        this.SelesGroupList = response.data
      })
    },
    editSalesGroupData() {
      this.$refs.editFormRef.validate(valid => {
        if (!valid) return
        // 发起请求
        UpdateGroupData({ groupID: this.editForm.groupID, groupName: this.editForm.groupName,
          leaderName: this.editForm.leaderName }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editDialogVisible = false
          this.GetGroupData()
          this.$message.success('更新成功')
        })
      })
    },
    addSalesGroupData() {
      this.$refs.addFormRef.validate(valid => {
        console.log('addForm此时的数据： ' + this.addForm.groupName)
        if (!valid) return
        InsertGroupData({ groupID: this.addForm.groupID, groupName: this.addForm.groupName,
          leaderName: this.addForm.leaderName }).then(response => {
          // if (response.code !== 200) {
          //   return this.$message.error('添加失败')
          // }
          this.addDialogVisible = false
          this.GetGroupData()
          this.$message.success('添加成功')
        })
      })
    },
    async showAddGroupDialog() {
      this.addDialogVisible = true
    },
    async showEditDialog(id) {
      GetUpdateData({ groupID: id }).then(response => {
        this.editForm = response.data[0]
      })
      this.editDialogVisible = true
    },
    DeleteSalesGroup(id) {
      this.$confirm('此操作将删除该销售员信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => { // 确定执行then方法)
        console.log('id是：' + id)
        RemoveGroupData({ groupID: id }).then(response => {
          if (response.code === 200) {
            this.$message({
              type: 'success',
              message: '删除成功!'
            })
            this.GetGroupData()
          }
        })
      }
      )
    },
    // async getCategoryAndCount() {
    //   const { data } = await findCategoryAndCount()
    //   this.petsCount = data.data.petsCount
    // },
    // 重置
    chongzhi() {
      this.req.salesmanID = ''
      this.req.salesmanName = ''
      this.req.salesmanPhone = ''
      this.req.groupID = ''
      this.req.salesmanIDcard = ''
      this.getPetList()
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
