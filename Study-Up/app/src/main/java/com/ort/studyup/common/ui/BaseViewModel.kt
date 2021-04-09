package com.ort.studyup.common.ui

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.services.ServiceError
import com.ort.studyup.services.ServiceException
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.coroutineScope
import kotlinx.coroutines.launch

open class BaseViewModel : ViewModel() /*,KoinComponent*/ {

    val error: MutableLiveData<ServiceError> = MutableLiveData()

    val loading: MutableLiveData<CustomLoader.LoaderConfig> = MutableLiveData()

    protected fun executeService(loader: CustomLoader.Visibility = CustomLoader.Visibility.TRANSLUCENT, service: suspend () -> Unit) {
        loading.postValue(CustomLoader.LoaderConfig(true, loader))
        GlobalScope.launch {
            coroutineScope {
                try {
                    service()
                } catch (ex: ServiceException) {
                    ex.printStackTrace()
                    error.postValue(ex.error)
                } catch (ex: Exception) {
                    ex.printStackTrace()
                    error.postValue(ServiceError(INTERNAL_ERROR_CODE, ex.message ?: ""))
                }
                loading.postValue(CustomLoader.LoaderConfig(false))
            }
        }
    }

}