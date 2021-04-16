package com.ort.studyup.login

import android.os.Bundle
import android.text.InputType
import android.text.method.PasswordTransformationMethod
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.Observer
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import com.ort.studyup.R
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_login.*
import kotlinx.android.synthetic.main.item_text_input.view.*

class LoginFragment : BaseFragment() {

    private val viewModel: LoginViewModel by injectViewModel(LoginViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_login, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()

    }

    private fun initUI() {
        logInButton.setOnClickListener {
            viewModel.login(emailInput.text.toString(), passwordInput.text.toString()).observe(
                viewLifecycleOwner
            ) {
                if (it.isStudent) {
                    requireActivity().finish()
                    findNavController().navigate(R.id.action_loginFragment_to_studentHomeActivity)
                }
                else{
                    requireActivity().finish()
                    findNavController().navigate(R.id.action_loginFragment_to_teacherHomeActivity)
                }
            }
        }
        signUp.setOnClickListener {
            findNavController().navigate(R.id.action_loginFragment_to_registerFragment)
        }


    }

}