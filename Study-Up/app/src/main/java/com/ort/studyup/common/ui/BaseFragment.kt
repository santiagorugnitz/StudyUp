package com.ort.studyup.common.ui

import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import com.google.android.material.snackbar.Snackbar
import com.ort.studyup.R
import com.ort.studyup.services.ServiceError
import org.koin.android.viewmodel.ext.android.viewModel
import org.koin.core.parameter.ParametersDefinition
import org.koin.core.qualifier.Qualifier
import kotlin.reflect.KClass

open class BaseFragment : Fragment() {

    private val loader: CustomLoader by lazy { CustomLoader(requireContext(), null) }


    private val onError = Observer<ServiceError> {
        showError(it.code, if (it.toString().isEmpty()) getString(R.string.generic_error) else it.toString())
    }

    private val onLoad = Observer<CustomLoader.LoaderConfig> {
        if (it.show) loader.show(it.visibility) else loader.hide()
    }

    private fun showError(code: Int, message: String) {
        val snackBar = Snackbar.make(this.requireView(), message, Snackbar.LENGTH_LONG)
        snackBar.view.setBackgroundColor(ContextCompat.getColor(this.requireContext(), R.color.error))
        snackBar.show()
    }

    fun <T : BaseViewModel> injectViewModel(clazz: KClass<T>, qualifier: Qualifier? = null, parameters: ParametersDefinition? = null): Lazy<T> =
        lazy {
            val lazyViewModel = viewModel(clazz, qualifier, parameters)
            val viewModel = lazyViewModel.value
            viewModel.error.observe(this, onError)
            viewModel.loading.observe(this, onLoad)
            viewModel
        }

}