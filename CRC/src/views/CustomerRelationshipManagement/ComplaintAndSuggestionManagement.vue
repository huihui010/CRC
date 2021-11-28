<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item clearable label="投诉编号" label-width='120px'>
          <el-input v-model="req.contactName" placeholder="请输入投诉编号"></el-input>
        </el-form-item>
        <el-form-item label="处理状态" label-width='120px'>
          <el-select v-model="req.region" placeholder="请选择">
            <el-option label="未处理" value="未处理"></el-option>
            <el-option label="处理中" value="处理中"></el-option>
            <el-option label="已处理" value="已处理"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="getComplain">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddComplainDialog" v-if="!($store.getters.button.includes(7))">添加投诉记录</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <!--      表格-->
      <el-table
        :data="ComplainList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          prop="complainID"
          label="投诉编号"
          width="130">
        </el-table-column>
        <el-table-column
          prop="clientID"
          label="投诉客户编号"
          width="150">
        </el-table-column>
        <el-table-column
          prop="complainType"
          label="投诉类型"
          width="120">
        </el-table-column>
        <el-table-column
          prop="complainContent"
          label="投诉内容描述"
          width="130">
        </el-table-column>
        <el-table-column
          prop="complainFollowUp"
          label="跟进人"
          width="130">
        </el-table-column>
        <el-table-column
          prop="handler"
          label="处理人"
          width="130">
        </el-table-column>
        <el-table-column
          prop="handingResult"
          label="处理状态"
          width="130">
        </el-table-column>
        <el-table-column
          prop="handingExplain"
          label="处理结果"
          width="130">
        </el-table-column>
        <el-table-column
          label="操作"
          width="300">
          <template slot-scope="scope">
            <!--修改-->
            <el-button v-if="!($store.getters.button.includes(7))" type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.complainID)"></el-button>
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
        <el-form-item label="客户编号" prop="clientID">
          <el-input v-model="addForm.clientID" ></el-input>
        </el-form-item>
        <el-form-item label="投诉类型" prop="complainType">
          <el-select v-model="addForm.complainType" placeholder="请选择">
            <el-option label="描述不符" value="描述不符"></el-option>
            <el-option label="品牌描述不符" value="品牌描述不符"></el-option>
            <el-option label="违背承诺" value="违背承诺"></el-option>
            <el-option label="售假" value="售假"></el-option>
            <el-option label="违反广告" value="违反广告"></el-option>
            <el-option label="服务不佳" value="服务不佳"></el-option>
            <el-option label="其它" value="其它"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="内容描述" prop="complainContent">
          <el-input type="textarea" v-model="addForm.complainContent"></el-input>
        </el-form-item>
        <el-form-item label="跟进者" prop="complainFollowUp">
          <el-input v-model="addForm.complainFollowUp"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addCompalianData">确 定</el-button>
      </span>
    </el-dialog>
    <el-dialog
      title="修改信息"
      :visible.sync="editDialogVisible"
      width="35%"
      @close="editDialogClosed">
      <el-form :model="editForm" :rules="editFormRules" ref="editFormRef" label-width="100px">
        <el-form-item label="投诉编号">
          <el-input v-model="editForm.complainID" disabled></el-input>
        </el-form-item>
        <el-form-item label="客户编号">
          <el-input v-model="editForm.clientID" disabled></el-input>
        </el-form-item>
        <el-form-item label="投诉类型">
          <el-input v-model="editForm.complainType" disabled></el-input>
        </el-form-item>
        <el-form-item label="投诉内容">
          <el-input type="textarea" v-model="editForm.complainContent" disabled></el-input>
        </el-form-item>
        <el-form-item label="跟进人">
          <el-input v-model="editForm.complainFollowUp" disabled></el-input>
        </el-form-item>
        <el-form-item label="处理人">
          <el-input v-model="editForm.handler"></el-input>
        </el-form-item>
        <el-form-item label="处理状态">
          <el-select v-model="editForm.handingResult" placeholder="请选择">
            <el-option label="未处理" value="未处理"></el-option>
            <el-option label="处理中" value="处理中"></el-option>
            <el-option label="已处理" value="已处理"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="处理结果">
          <el-input v-model="editForm.handingExplain"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editComplainData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// import { findPetList } from '@/api/pets'
// import { findCategoryAndCount } from '@/api/pets'
import { GetComplainData, InsertComplainData, GetUpdateData, UpdateComplainData } from '@/api/ComplainAndSuggestionManagement'

export default {
  name: 'SalesPerformanceManagement',
  data() {
    const checkcontactPhone = (rule, value, callback) => {
      if (value.length !== 11) {
        callback(new Error('电话号码位数不对'))
      } else {
        callback()
      }
    }
    const checkclientID = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入客户编号'))
      } else {
        callback()
      }
    }
    const checkcomplainType = (rule, value, callback) => {
      if (!value) {
        callback(new Error('请选择投诉类型'))
      } else {
        callback()
      }
    }
    const checkcomplainFollowUp = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入处理人姓名'))
      } else {
        callback()
      }
    }
    const checkcomplainContent = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入投诉详情描述'))
      } else {
        callback()
      }
    }
    return {
      req: {
        salesmanID: '',
        contactName: '',
        contactPhone: '',
        groupID: '',
        salesmanIDcard: ''
      },
      addFormRules: {
        complainType: [
          { required: true, message: '请选择投诉类型', trigger: 'blur' },
          { validator: checkcomplainType, trigger: 'blur' }
        ],
        clientID: [
          { required: true, message: '请输入客户编号', trigger: 'blur' },
          { validator: checkclientID, trigger: 'blur' }
        ],
        complainFollowUp: [
          { required: true, message: '请输入处理人姓名', trigger: 'blur' },
          { validator: checkcomplainFollowUp, trigger: 'blur' }
        ],
        complainContent: [
          { required: true, message: '请输入投诉详情描述', trigger: 'blur' },
          { validator: checkcomplainContent, trigger: 'blur' }
        ]
      },
      editFormRules: {
        contactPhone: [
          { required: true, message: '请输入联系人电话', trigger: 'blur' },
          { validator: checkcontactPhone, trigger: 'blur' }
        ]
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      ComplainList: [],
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
    this.getComplain()
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
    async getComplain() {
      GetComplainData().then(response => {
        this.ComplainList = response.data
      })
    },
    editComplainData() {
      this.$refs.editFormRef.validate(valid => {
        if (!valid) return
        // 发起请求
        UpdateComplainData({
          complainID: this.editForm.complainID,
          clientID: this.editForm.clientID,
          complainType: this.editForm.complainType,
          complainContent: this.editForm.complainContent,
          complainFollowUp: this.editForm.complainFollowUp,
          handler: this.editForm.handler,
          handingResult: this.editForm.handingResult,
          handingExplain: this.editForm.handingExplain
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editDialogVisible = false
          this.getComplain()
          this.$message.success('更新成功')
        })
      })
    },
    addCompalianData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertComplainData({
          clientID: this.addForm.clientID,
          complainType: this.addForm.complainType,
          complainContent: this.addForm.complainContent,
          complainFollowUp: this.addForm.complainFollowUp
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('添加失败')
          } else {
            this.addDialogVisible = false
            this.getComplain()
            this.$message.success('添加成功')
          }
        })
      })
    },
    async showAddComplainDialog() {
      this.addDialogVisible = true
    },
    async showEditDialog(id) {
      GetUpdateData({ complainID: id }).then(response => {
        this.editForm = response.data[0]
      })
      this.editDialogVisible = true
    },
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
