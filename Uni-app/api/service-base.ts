import { apiBaseUrl } from '@/common/http.api.js'
import store from '../store/index'
	
export class AbpServiceBase {

    protected transformResult(url: string, response, processor: (response) => Promise<any>): Promise<any> {
        console.log(response)
        return processor(response);
    }
}

export function requestPost<U = any>(option: RequestOptions<U>){
	option.method = 'post'
	return request(option)
}

export function requestPut<U = any>(option: RequestOptions<U>){
	option.method = 'put'
	return request(option)
}

export function request<U = any>(option: RequestOptions<U>) {

	if(typeof(option) != 'object')
		option = {}
	
	if(option.url.indexOf(apiBaseUrl) < 0) {
		option.url = apiBaseUrl + option.url
	}
	if (!option.header) {
	    option.header = {}
	}
	if(!option.header.Authorization){
		if(store.state.vuex_token){
			option.header['Authorization'] = `Bearer ${store.state.vuex_token.access_token}`
		}
	}
	// 默认语言
	if(!option.header['Accept-Language']){
		option.header['Accept-Language'] = store.state.vuex_lang || `zh-Hans,zh-CN,zh;q=0.9`
	}
	// 默认租户
	if(store.state.vuex_tenant_id && !option.header['Tenant-Id']){	
		option.header['Tenant-Id'] =  store.state.vuex_tenant_id
	}
		
	// 异常处理
	if(option.fail){
		const func = option.fail
		option.fail = (err) => {
			uni.hideLoading()
			console.log(err)
			fun(err)
		}
	}else{
		option.fail = function(err){
			uni.hideLoading()
			console.log(option.url, err)
			if(err && err.errMsg){
				uni.showToast({
					title: err.errMsg,
					icon: 'none',
					duration: 3500
				})
			}
			throw new Error(`request failed: ${option.url}, \r\n${JSON.stringify(option)}`)
		}
	}

	const promise = new Promise((resolve, reject)=>{
		
		if(option.success){
			const fun = option.success
			option.success = (response) => {
				console.log(response)
				fun(response)
				resolve(response)
			}
		} else {
			option.success = (response) => {
				const { error,error_description } = response.data
				if(error && error_description){
					reject(response.data)
				}else if(response.statusCode !== 200){
					const { error: { message, details } } = response.data
					uni.hideLoading()
					uni.showModal({
						title: message || '请求错误',
						content: details,
						showCancel: false
					})
				}else{
					resolve(response)
				}
			}
		}
		uni.request(option)
	})
	console.log("option=>", option)
    return promise
}