<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item clearable label="客户编号" label-width='90px'>
          <el-input v-model="req.clientID" placeholder="请输入客户编号"></el-input>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="getClient">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddClientDialog">添加</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <!--      表格-->
      <el-table
        :data="ClientList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          fixed
          prop="clientID"
          label="客户编号"
          width="100">
        </el-table-column>
        <el-table-column
          prop="clientName"
          label="客户姓名"
          width="100">
        </el-table-column>
        <el-table-column
          prop="clientAddress"
          label="客户电话"
          width="100">
        </el-table-column>
        <el-table-column
          prop="salesmanID"
          label="所属销售员编号"
          width="120">
        </el-table-column>
        <el-table-column
          prop="clientCharge"
          label="客户负责人"
          width="100">
        </el-table-column>
        <el-table-column
          prop="clientProfession"
          label="客户职业"
          width="100">
        </el-table-column>
        <el-table-column
          prop="clientPhone"
          label="客户电话"
          width="100">
        </el-table-column>
        <el-table-column
          prop="clientPicture"
          label="客户照片"
          width="100">
          <!--          <template slot-scope='scope' >-->
          <!--            <img :src="scope.row.petImage + '?x-oss-process=image/resize,w_150 '">-->
          <!--          </template>-->
          <template slot-scope="scope">
            <img :src="scope.row.clientPicture + '?x-oss-process=image/resize,w_40 '" @click="previewImg(scope.row.clientPicture)">
          </template>
        </el-table-column>
        <el-table-column
          prop="clientFavorite"
          label="客户喜好"
          width="100">
        </el-table-column>
        <el-table-column
          prop="clientHobby"
          label="客户兴趣"
          width="100">
        </el-table-column>
        <el-table-column
          prop="remark"
          label="备注"
          width="100">
        </el-table-column>
        <el-table-column
          fixed="right"
          label="操作"
          width="200">
          <template slot-scope='scope'>
            <!--修改-->
            <el-button type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.clientID)"></el-button>
            <!--删除-->
            <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeleteClient(scope.row.clientID)"></el-button>
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
        <el-form-item label="客户姓名" prop="clientName">
          <el-input v-model="addForm.clientName"></el-input>
        </el-form-item>
        <el-form-item label="客户地址" prop="clientAddress">
          <el-input v-model="addForm.clientAddress"></el-input>
        </el-form-item>
        <el-form-item label="客户负责人" prop="clientCharge">
          <el-input v-model="addForm.clientCharge"></el-input>
        </el-form-item>
        <el-form-item label="客户所属销售员编号" prop="salesmanID">
          <el-input v-model="addForm.salesmanID"></el-input>
        </el-form-item>
        <el-form-item label="客户职业">
          <el-input v-model="addForm.clientProfession"></el-input>
        </el-form-item>
        <el-form-item label="客户照片">
          <el-input v-model="addForm.clientPicture"></el-input>
        </el-form-item>
        <el-form-item label="客户电话" prop="clientPhone">
          <el-input v-model="addForm.clientPhone"></el-input>
        </el-form-item>
        <el-form-item label="客户喜好">
          <el-input v-model="addForm.clientFavorite"></el-input>
        </el-form-item>
        <el-form-item label="客户兴趣">
          <el-input v-model="addForm.clientHobby"></el-input>
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="addForm.remark"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addClientsData">确 定</el-button>
      </span>
    </el-dialog>
    <el-dialog
      title="修改信息"
      :visible.sync="editDialogVisible"
      width="35%"
      @close="editDialogClosed">
      <el-form :model="editForm" :rules="editFormRules" ref="editFormRef" label-width="100px">
        <el-form-item label="客户编号">
          <el-input v-model="editForm.clientID" disabled></el-input>
        </el-form-item>
        <el-form-item label="客户所属销售员编号">
          <el-input v-model="editForm.salesmanID" disabled></el-input>
        </el-form-item>
        <el-form-item label="客户姓名">
          <el-input v-model="editForm.clientName" disabled></el-input>
        </el-form-item>
        <el-form-item label="客户地址">
          <el-input v-model="editForm.clientAddress"></el-input>
        </el-form-item>
        <el-form-item label="客户负责人" prop="clientCharge">
          <el-input v-model="editForm.clientCharge"></el-input>
        </el-form-item>
        <el-form-item label="客户职业">
          <el-input v-model="editForm.clientProfession"></el-input>
        </el-form-item>
        <el-form-item label="客户照片">
          <el-input v-model="editForm.clientPicture"></el-input>
        </el-form-item>
        <el-form-item label="客户电话" prop="clientPhone">
          <el-input v-model="editForm.clientPhone"></el-input>
        </el-form-item>
        <el-form-item label="客户喜好">
          <el-input v-model="editForm.clientFavorite"></el-input>
        </el-form-item>
        <el-form-item label="客户兴趣">
          <el-input v-model="editForm.clientHobby"></el-input>
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="editForm.remark"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editClientsData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// import { findPetList } from '@/api/pets'
// import { findCategoryAndCount } from '@/api/pets'
import { GetClientData, InsertClientData, UpdateClientData, RemoveClientData, GetUpdateData } from '@/api/ClientManagement'

export default {
  name: 'ClientManagement',
  data() {
    const checkclientCharge = (rule, value, callback) => {
      if (value.length === null) {
        callback(new Error('请输入客户编号'))
      } else {
        callback()
      }
    }
    const checkclientAddress = (rule, value, callback) => {
      if (value.length === null) {
        callback(new Error('请输入客户编号'))
      } else {
        callback()
      }
    }
    const checksalesmanID = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入客户所属销售员编号'))
      } else {
        callback()
      }
    }
    const checkClientName = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入客户姓名'))
      } else {
        callback()
      }
    }
    const checkClientPhone = (rule, value, callback) => {
      if (value.length !== 11) {
        callback(new Error('电话号码位数不正确'))
      } else {
        callback()
      }
    }
    return {
      req: {
        clientID: '',
        clientName: '',
        clientAddress: '',
        clientCharge: '',
        clientProfession: '',
        clientPicture: '',
        clientPhone: ''
      },
      addFormRules: {
        clientCharge: [
          { required: true, message: '请输入客户负责人', trigger: 'blur' },
          { validator: checkclientCharge, trigger: 'blur' }
        ],
        clientName: [
          { required: true, message: '请输入客户姓名', trigger: 'blur' },
          { validator: checkClientName, trigger: 'blur' }
        ],
        clientAddress: [
          { required: true, message: '请输入客户地址', trigger: 'blur' },
          { validator: checkclientAddress, trigger: 'blur' }
        ],
        clientPhone: [
          { required: true, message: '请输入客户手机号', trigger: 'blur' },
          { validator: checkClientPhone, trigger: 'blur' }
        ],
        salesmanID: [
          { required: true, message: '请输入客户所属销售员编号', trigger: 'blur' },
          { validator: checksalesmanID, trigger: 'blur' }
        ]
      },
      editFormRules: {
        clientPhone: [
          { required: true, message: '请输入手机号', trigger: 'blur' },
          { validator: checkClientPhone, trigger: 'blur' }
        ],
        clientCharge: [
          { required: true, message: '请输入客户负责人', trigger: 'blur' },
          { validator: checkclientCharge, trigger: 'blur' }
        ]
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      ClientList: [],
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
    this.getClient()
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
    async getClient() {
      GetClientData().then(response => {
        this.ClientList = response.data
      })
    },
    editClientsData() {
      this.$refs.editFormRef.validate(valid => {
        if (!valid) return
        // 发起请求
        UpdateClientData({ clientID: this.editForm.clientID,
          salesmanID: this.editForm.salesmanID,
          clientName: this.editForm.clientName,
          clientAddress: this.editForm.clientAddress,
          clientCharge: this.editForm.clientCharge,
          clientProfession: this.editForm.clientProfession,
          clientPicture: this.editForm.clientPicture,
          clientPhone: this.editForm.clientPhone,
          clientFavorite: this.editForm.clientFavorite,
          clientHobby: this.editForm.clientHobby,
          remark: this.editForm.remark
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editDialogVisible = false
          this.getClient()
          this.$message.success('更新成功')
        })
      })
    },
    addClientsData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertClientData({ clientName: this.addForm.clientName,
          salesmanID: this.addForm.salesmanID,
          clientAddress: this.addForm.clientAddress,
          clientCharge: this.addForm.clientCharge,
          clientProfession: this.addForm.clientProfession,
          clientPicture: this.addForm.clientPicture,
          clientPhone: this.addForm.clientPhone,
          clientFavorite: this.addForm.clientFavorite,
          clientHobby: this.addForm.clientHobby,
          remark: this.addForm.remark
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('添加失败')
          }
          this.addDialogVisible = false
          this.getClient()
          this.$message.success('添加成功')
        })
      })
    },
    async showAddClientDialog() {
      this.addDialogVisible = true
    },
    async showEditDialog(id) {
      GetUpdateData({ clientID: id }).then(response => {
        this.editForm = response.data[0]
      })
      this.editDialogVisible = true
    },
    DeleteClient(id) {
      this.$confirm('此操作将删除该用户信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => { // 确定执行then方法)
        RemoveClientData({ clientID: id }).then(response => {
          if (response.code === 200) {
            this.$message({
              type: 'success',
              message: '删除成功!'
            })
            this.getClient()
          }
        })
      }
      )
    },
    // 重置
    chongzhi() {
      this.req.clientID = ''
      this.req.clientName = ''
      this.req.clientAddress = ''
      this.req.clientCharge = ''
      this.req.clientProfession = ''
      this.req.clientPicture = ''
      this.req.clientPhone = ''
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
