package com.ort.studyup.home.groups

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.fragment.findNavController
import com.ort.studyup.R
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_new_group.*

class NewGroupFragment : BaseFragment() {

    private val viewModel: NewGroupViewModel by injectViewModel(NewGroupViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_new_group, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        saveButton.setOnClickListener {
            viewModel.createGroup(nameInput.text.toString()).observe(viewLifecycleOwner, {
                if (it) {
                    findNavController().navigate(R.id.action_newGroupFragment_to_groupsFragment)
                }
            })
        }
    }
}