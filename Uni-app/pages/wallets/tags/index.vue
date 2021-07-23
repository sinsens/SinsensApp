<template>
	<view>
		<u-navbar title="标签">
			<view class="u-navbar-right" slot="right">
				<u-icon name="plus" @tap="showCreateModal = true"></u-icon>
			</view>
		</u-navbar>
		<u-card title="标签">
			<view class="" slot="body">
				<view v-for="(item,index) in tags" :key="index" class="u-body-item u-flex u-row-between u-p-b-0">
					<view @click="updateTag=item;showUpdateModal=true" class="u-body-item-title u-line-2">{{item.title}}
					</view>
				</view>
			</view>
		</u-card>
		<u-modal v-model="showCreateModal" ref="createModal" title="新建标签" @confirm="onCreate" @cancel="newTagTitle = ''"
			:async-close="true" confirm-text="保存" :show-cancel-button="true">
			<u-form-item label="主题" class="slot-content">
				<u-input maxlength="32" v-model="newTagTitle"></u-input>
			</u-form-item>
		</u-modal>
		<u-modal v-model="showUpdateModal" ref="updateModal" title="修改标签" @confirm="onUpdate" @cancel="refresh"
			:async-close="true" confirm-text="保存" :show-cancel-button="true">
			<view class="slot-content">
				<u-form-item label="主题">
					<u-input maxlength="32" v-model="updateTag.title"></u-input>
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
	export default {
		onShow() {
			this.refresh()
		},
		data() {
			return {
				tags: [],
				newTagTitle: '',
				showCreateModal: false,
				showUpdateModal: false,
				updateTag: {
					title: ''
				}
			}
		},
		computed: {},
		methods: {
			refresh() {
				request({
					url: '/api/app/tag?SkipCount=0&MaxResultCount=100'
				}).then(result => {
					console.log(result)
					this.tags = result.data.items
				})
			},
			onCreate() {
				if (this.newTagTitle.length < 1) {
					uni.showToast({
						icon: 'none',
						title: '标签主题不能为空'
					})
					this.$refs.createModal.clearLoading()
					return
				}
				requestPost({
					url: '/api/app/tag',
					data: {
						title: this.newTagTitle
					}
				}).then(result => {
					if (result.data.id) {
						uni.showToast({
							title: '保存成功',
							icon: 'none'
						})
						this.newTagTitle = ''
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
				if (this.updateTag.title.length < 1) {
					uni.showToast({
						icon: 'none',
						title: '标签主题不能为空'
					})
					this.$refs.updateModal.clearLoading()
					return
				}
				requestPut({
					url: `/api/app/tag/${this.updateTag.id}`,
					data: this.updateTag
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
