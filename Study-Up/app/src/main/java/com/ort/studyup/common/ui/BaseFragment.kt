package com.ort.studyup.common.ui

import android.app.Activity
import android.view.View
import android.view.inputmethod.InputMethodManager
import android.widget.ArrayAdapter
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import com.google.android.material.snackbar.Snackbar
import com.ort.studyup.R
import com.ort.studyup.services.ServiceError
import kotlinx.android.synthetic.main.item_spinner.view.*
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

    protected fun initSpinner(input: View, array: Array<String>, title: String) {
        ArrayAdapter(requireActivity(), android.R.layout.simple_spinner_item, array).also {
            it.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
            input.spinner.adapter = it
        }
        input.spinnerTitle.text = title
    }

    protected fun hideKeyboard() {
        activity?.let {
            val imm = it.getSystemService(Activity.INPUT_METHOD_SERVICE) as InputMethodManager
            var view = it.currentFocus
            if (view == null) {
                view = View(it)
            }
            imm.hideSoftInputFromWindow(view.windowToken, 0)
        }
    }

}