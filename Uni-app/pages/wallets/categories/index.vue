<template>
	<view>
		<u-navbar title="分类">
			<view class="u-navbar-right" slot="right">
				<u-icon name="plus" @tap="showCreateModal = true"></u-icon>
			</view>
		</u-navbar>
		<u-card title="支出分类">
			<view class="" slot="body">
				<view v-for="(item,index) in categories.filter(x=>x.transactionType == 1)" :key="index"
					class="u-body-item u-flex u-row-between u-p-b-0">
					<view @click="handleChange(item)" class="u-body-item-title u-line-2">
						<span class="color-preview" :style="{background:numberToColor(item.color)}"></span>
						{{item.title}}
					</view>
				</view>
			</view>
		</u-card>
		<u-card title="收入分类">
			<view class="" slot="body">
				<view v-for="(item,index) in categories.filter(x=>x.transactionType == 2)" :key="index"
					class="u-body-item u-flex u-row-between u-p-b-0">
					<view @click="handleChange(item)" class="u-body-item-title u-line-2">
						<span class="color-preview" :style="{background:numberToColor(item.color)}"></span>
						{{item.title}}
					</view>
				</view>
			</view>
		</u-card>
		<u-card title="转账分类">
			<view class="" slot="body">
				<view v-for="(item,index) in categories.filter(x=>x.transactionType == 3)" :key="index"
					class="u-body-item u-flex u-row-between u-p-b-0">
					<view @click="handleChange(item)" class="u-body-item-title u-line-2">
						<span class="color-preview" :style="{background:numberToColor(item.color)}"></span>
						{{item.title}}
					</view>
				</view>
			</view>
		</u-card>
		<u-modal v-model="showCreateModal" ref="createModal" title="新建分类" @confirm="onCreate"
			@cancel="newCategory.title = ''" :async-close="true" confirm-text="保存" :show-cancel-button="true">
			<view class="slot-content">
				<u-form-item label="主题">
					<u-input maxlength="32" v-model="newCategory.title"></u-input>
				</u-form-item>
				<u-form-item label="类别">
					<u-select v-model="showNewCategoryType" :list="list" @confirm="selectConfirm"></u-select>
					{{newCategory.transactionTypeLabel}}
					<u-button @tap="showNewCategoryType = true" size="mini">选择</u-button>
				</u-form-item>
				<u-form-item label="颜色">
					<ColorPicker :show="showNewCategoryColor" @change="onCreateColorChange"></ColorPicker>
					<view v-if="!showNewCategoryColor"
						:style="{width:'50px',height:'50px',background:newCategory.colorStyle}"></view>
					<u-button @tap="showNewCategoryColor = !showNewCategoryColor" size="mini">
						{{showNewCategoryColor ? '确定' : '选择'}}
					</u-button>
				</u-form-item>

			</view>
		</u-modal>
		<u-modal v-model="showUpdateModal" ref="updateModal" title="修改分类" @confirm="onUpdate" @cancel="refresh"
			:async-close="true" confirm-text="保存" :show-cancel-button="true">
			<view class="slot-content">
				<u-form-item label="主题">
					<u-input maxlength="32" v-model="updateModel.title"></u-input>
				</u-form-item>
				<u-form-item label="颜色">
					<ColorPicker :orignColor="updateModel.color" :show="showUpdateCategoryColor"
						@change="onUpdateColorChange"></ColorPicker>
					<view v-if="!showUpdateCategoryColor"
						:style="{width:'50px',height:'50px',background:updateModel.colorStyle}"></view>
					<u-button @tap="showUpdateCategoryColor = !showUpdateCategoryColor" size="mini">
						{{showUpdateCategoryColor ? '确定' : '选择'}}
					</u-button>
				</u-form-item>
			</view>

		</u-modal>
	</view>
</template>

<script>
	import {
		request,
		requestPost,
		requestPut
	} from '@/api/service-base'

	import Color from '@/common/color'
	import ColorPicker from '@/components/color-picker'

	export default {
		components: {
			ColorPicker
		},
		onShow() {
			this.refresh()
		},
		data() {
			return {
				list: [{
					value: 1,
					label: '支出'
				}, {
					value: 2,
					label: '收入'
				}, {
					value: 3,
					label: '转账'
				}],
				showNewCategoryType: false,
				showNewCategoryColor: false,
				showUpdateCategoryColor: false,
				categories: [],
				newCategory: {
					title: '',
					transactionType: 1,
					newCategoryTypeLabel: '支出',
				},
				showCreateModal: false,
				showUpdateModal: false,
				updateModel: {
					title: ''
				}
			}
		},
		computed: {},
		methods: {
			numberToColor(val) {
				return (val != undefined && val != null) ? '#' + Color.numberToHex(val) : 'white'
			},
			refresh() {
				request({
					url: '/api/app/category?SkipCount=0&MaxResultCount=100'
				}).then(result => {
					console.log(result)
					this.categories = result.data.items
				})
			},
			selectConfirm(e) {
				console.log('selectConfirm', e)
				this.newCategory.transactionTypeLabel = e[0].label
				this.newCategory.transactionType = e[0].value
			},
			onCreateColorChange(e) {
				console.log('onCreateColorChange', e)
				this.newCategory.color = e.number
				this.newCategory.colorStyle = e.color
				console.log(this.newCategory)
			},
			onUpdateColorChange(e) {
				console.log('onUpdateColorChange', e)
				this.updateModel.color = e.number
				this.updateModel.colorStyle = e.color
				console.log(this.updateModel)
			},
			handleChange(e) {
				this.showUpdateModal = true
				this.updateModel = e
				this.updateModel.colorStyle = this.numberToColor(e.color)
			},
			onCreate() {
				if (this.newCategory.title.length < 1) {
					uni.showToast({
						icon: 'none',
						title: '主题不能为空'
					})
					this.$refs.createModal.clearLoading()
					return
				}
				requestPost({
					url: '/api/app/category',
					data: this.newCategory
				}).then(result => {
					if (result.data.id) {
						uni.showToast({
							title: '保存成功',
							icon: 'none'
						})
						this.newCategory.title = ''
						this.newCategory.transactionType = 1
						this.newCategory.transactionTypeLabel = '支出',
							this.refresh()
					}
					this.showCreateModal = false
				}).catch(err => {
					this.showCreateModal = false
					uni.showToast({
						title: err.error_description || err.error || err || '保存失败',
						icon: 'none'
					})
				})
			},
			onUpdate() {
				if (this.updateModel.title.length < 1) {
					uni.showToast({
						icon: 'none',
						title: '主题不能为空'
					})
					this.$refs.updateModal.clearLoading()
					return
				}
				requestPut({
					url: `/api/app/category/${this.updateModel.id}`,
					data: this.updateModel
				}).then(result => {
					if (result.data.id) {
						uni.showToast({
							title: '保存成功',
							icon: 'none'
						})
						this.refresh()
					}
					this.showUpdateModal = false
				}).catch(err => {
					this.showUpdateModal = false
					uni.showToast({
						title: err.error_description || err.error || err || '保存失败',
						icon: 'none'
					})
				})
			}
		}
	}
</script>

<style scoped lang="scss">
	.color-preview {
		display: inline-block;
		vertical-align: bottom;
		margin-right: 20rpx;
		width: 40rpx;
		height: 40rpx;
	}

	.u-navbar-right {
		margin-right: 20px;
	}

	.u-card-wrap {
		background-color: $u-bg-color;
		padding: 1px;
	}

	.u-body-item {
		font-size: 32rpx;
		color: #333;
		padding: 20rpx 10rpx;
	}

	.slot-content {
		padding-left: 50px;
	}
</style>
