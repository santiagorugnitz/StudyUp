package com.ort.studyup.home.exams

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import com.ort.studyup.R
import com.ort.studyup.common.EXAM_ID_KEY
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_new_exam.*
import kotlinx.android.synthetic.main.item_spinner.view.*

class NewExamFragment : BaseFragment() {

    private val viewModel: NewExamViewModel by injectViewModel(NewExamViewModel::class)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_new_exam, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        initSpinner(
            difficultyInput,
            resources.getStringArray(R.array.difficulties),
            getString(R.string.difficulty)
        )

        saveButton.setOnClickListener {
            viewModel.sendData(
                nameInput.text.toString(),
                subjectInput.text.toString(),
                difficultyInput.spinner.selectedItemPosition
            ).observe(viewLifecycleOwner, {
                if (it > 0) {
                    findNavController().navigate(
                        R.id.action_newExamFragment_to_examsFragment,
                        Bundle().apply { putInt(EXAM_ID_KEY, it) })
                }
            })
        }
    }

}