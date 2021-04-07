package com.ort.studyup.login

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_login.*
import kotlinx.android.synthetic.main.item_text_input.view.*

class LoginFragment : BaseFragment() {

    private val viewModel: LoginViewModel = LoginViewModel()

    lateinit var button : View

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_login,container,false)

        button.setOnClickListener {
            if(viewModel.validate()){
                viewModel.login(username.textInputEditText.text.toString(),password.textInputEditText.text.toString())
            }
        }
    }

}