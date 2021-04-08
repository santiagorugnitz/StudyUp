package com.ort.studyup.login

import android.os.Bundle
import android.text.InputType
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.Observer
import com.ort.studyup.R
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_login.*
import kotlinx.android.synthetic.main.fragment_login.view.*
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
        username.titleInput.text = getString(R.string.username_or_mail)
        password.titleInput.text = getString(R.string.password)
        password.textInputEditText.inputType = InputType.TYPE_TEXT_VARIATION_PASSWORD
        logInButton.setOnClickListener {
            viewModel.login(username.textInputEditText.text.toString(), password.textInputEditText.text.toString()).observe(
                viewLifecycleOwner, {
                    if (it) {
                        //TODO: navigate to home
                    }
                }
            )
        }
        signUp.setOnClickListener {
            //TODO: navigate to register
        }


    }

}