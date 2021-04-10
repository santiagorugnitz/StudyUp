package com.ort.studyup.login

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import com.ort.studyup.R
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_new_deck.*
import kotlinx.android.synthetic.main.fragment_register.*
import kotlinx.android.synthetic.main.item_spinner.view.*

class RegisterFragment : BaseFragment() {

    private val viewModel: RegisterViewModel by injectViewModel(RegisterViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_register, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initSpinner(roleInput, resources.getStringArray(R.array.roles), getString(R.string.role))
        registerButton.setOnClickListener {
            viewModel.register(
                usernameInput.text.toString(),
                emailInput.text.toString(),
                passwordInput.text.toString(),
                confirmPasswordInput.text.toString(),
                roleInput.spinner.selectedItemPosition == 0
            ).observe(viewLifecycleOwner) {
                if (it) {
                    requireActivity().finish()
                    findNavController().navigate(R.id.action_registerFragment_to_homeActivity)
                }
            }
        }
    }


}