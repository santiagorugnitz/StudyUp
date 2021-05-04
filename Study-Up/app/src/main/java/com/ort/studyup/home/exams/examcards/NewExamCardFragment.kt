package com.ort.studyup.home.exams.examcards

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.Observer
import com.ort.studyup.R
import com.ort.studyup.common.*
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.common.ui.ConfirmationDialog
import kotlinx.android.synthetic.main.fragment_new_flashcard.*

class NewExamCardFragment : BaseFragment(), ConfirmationDialog.Callback {

    private val viewModel: NewExamCardViewModel by injectViewModel(NewExamCardViewModel::class)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_new_flashcard, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        val examId = arguments?.getInt(EXAM_ID_KEY)

        arguments?.getInt(EXAM_CARD_ID_KEY)?.let {
            if (it != 0) {
                viewModel.examCardId = it
                arguments?.getString(QUESTION_KEY)?.let {
                    questionInput.setText(it)
                }
                arguments?.getString(ANSWER_KEY)?.let {
                    //TODO: set selected in spinner
                }

                saveButton.text = getString(R.string.save_changes)
                header.text = getString(R.string.edit_examcard)
                deleteButton.visibility = View.VISIBLE
                deleteButton.setOnClickListener {
                    ConfirmationDialog(
                        requireContext(),
                        getString(R.string.delete_exam_card_confirmation),
                        this,
                    ).show()
                }
            }
        }

        saveButton.setOnClickListener {
            viewModel.sendData(
                examId ?: 0,
                questionInput.text.toString(),
                //TODO: get selected from spinner
            ).observe(viewLifecycleOwner, Observer {
                if (it > 0)
                    requireActivity().onBackPressed()
            })
        }

    }

    override fun onButtonClick() {
        viewModel.deleteExamCard().observe(viewLifecycleOwner, Observer {
            if (it) {
                requireActivity().onBackPressed()
                requireActivity().onBackPressed()
            }
        })
    }
}