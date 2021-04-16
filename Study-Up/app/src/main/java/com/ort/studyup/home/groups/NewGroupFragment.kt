package com.ort.studyup.home.groups

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.fragment.findNavController
import com.ort.studyup.R
import com.ort.studyup.common.DECK_DATA_KEY
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.common.ui.ConfirmationDialog
import kotlinx.android.synthetic.main.fragment_new_deck.*
import kotlinx.android.synthetic.main.item_spinner.view.*

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

    }
}