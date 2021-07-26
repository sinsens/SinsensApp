<template>
	<u-modal :title="title || '请选择分类'" :show-confirm-button="false" v-model="show" :mask-close-able="true">
		<u-cell-group title="支出">
			<u-cell-item :arrow="false" v-for="(item,index) in categories.filter(x=>x.transactionType == 1)"
				:key="item.id">
				<view slot="title">
					<span class="color-preview" :style="{background:numberToColor(item.color)}"></span>
					{{item.title}}
				</view>
				<u-button @click="select(item)" size="mini">{{selectText || '选择'}}</u-button>
			</u-cell-item>
		</u-cell-group>

		<u-cell-group title="收入">
			<u-cell-item :arrow="false" v-for="(item,index) in categories.filter(x=>x.transactionType == 2)"
				:key="item.id">
				<view slot="title">
					<span class="color-preview" :style="{background:numberToColor(item.color)}"></span>
					{{item.title}}
				</view>
				<u-button @click="select(item)" size="mini">{{selectText || '选择'}}</u-button>
			</u-cell-item>
		</u-cell-group>

		<u-cell-group title="转账">
			<u-cell-item :arrow="false" v-for="(item,index) in categories.filter(x=>x.transactionType == 3)"
				:key="item.id">
				<view slot="title">
					<span class="color-preview" :style="{background:numberToColor(item.color)}"></span>
					{{item.title}}
				</view>
				<u-button @click="select(item)" size="mini">{{selectText || '选择'}}</u-button>
			</u-cell-item>
		</u-cell-group>
	</u-modal>
</template>

<script>
	/**
	 * @property {String} title = 显示标题
	 * @property {String} selectText = 选择按钮标题
	 * @property {Boolean} show = [false] 是否显示
	 */
	import {
		request
	} from '@/api/service-base'
	export default {
		name: "CategoryPicker",
		props: {
			title: String,
			selectText: String,
			show: Boolean = false
		},
		created() {
			request({
				url: '/api/app/category?SkipCount=0&MaxResultCount=100'
			}).then(result => {
				console.log(result)
				this.categories = result.data.items
			})
		},
		data() {
			return {
				categories: []
			}
		},
		methods: {
			numberToHex(num) {
				return (16581375 - Math.abs(num || 0)).toString(16)
			},
			numberToColor(val) {
				return (val != undefined && val != null) ? '#' + this.numberToHex(val) : 'white'
			},
			select(item) {
				this.$emit('select', item)
			}
		}
	}
</script>

<style scoped lang="scss">
	.color-preview {
		display: inline-block;
		vertical-align: middle;
		margin-right: 20rpx;
		width: 40rpx;
		height: 40rpx;
	}

	.slot-content {
		padding-left: 50px;
	}
</style>
