package com.ort.studyup.test

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.activity.addCallback
import androidx.navigation.fragment.findNavController
import com.ort.studyup.R
import com.ort.studyup.common.EXAM_ID_KEY
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.ExamCard
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.common.ui.ConfirmationDialog
import kotlinx.android.synthetic.main.fragment_test.*

class TestFragment : BaseFragment(), ConfirmationDialog.Callback {

    private val viewModel: TestViewModel by injectViewModel(TestViewModel::class)
    private lateinit var currentCard: ExamCard
    private lateinit var dialog: ConfirmationDialog
    private var showingDialog = false

    private var total = 0
    private var count = 0
    private var shouldExit = false

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_test, container, false)
    }

    override fun onStart() {
        super.onStart()
        timer.start()
        if (shouldExit) {
            findNavController().navigate(R.id.action_testFragment_to_preTestFragment)
        }
    }

    override fun onPause() {
        super.onPause()
        val time = timer.text.toString().split(":")
        val seconds = (time[1].toIntOrNull() ?: 0) + (time[0].toIntOrNull() ?: 0) * 60
        viewModel.sendAnswers(seconds)
        shouldExit = true
    }


    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        val examId = requireActivity().intent.extras?.getInt(EXAM_ID_KEY)
        examId?.let {
            initViewModel(examId)
            initUI()
        } ?: run {
            findNavController().navigate(R.id.action_testFragment_to_preTestFragment)
        }
        dialog = ConfirmationDialog(
            requireContext(),
            getString(R.string.end_test),
            this
        )
        requireActivity().onBackPressedDispatcher.addCallback(viewLifecycleOwner) {
            if (showingDialog) {
                dialog.hide()
                showingDialog = false
            } else {
                dialog.show()
                showingDialog = true
            }
        }

    }

    private fun initViewModel(deckId: Int) {
        viewModel.loadCards(deckId).observe(viewLifecycleOwner, {
            it?.let {
                total = viewModel.getTotal()
                onNewCard(it)
            } ?: run {
                showError(INTERNAL_ERROR_CODE, getString(R.string.no_questions_error))
                findNavController().navigate(R.id.action_testFragment_to_preTestFragment)
            }
        })
    }

    private fun initUI() {
        falseButton.setOnClickListener {
            viewModel.onAnswer(false).observe(viewLifecycleOwner, {
                onNewCard(it)
            })
        }
        trueButton.setOnClickListener {
            viewModel.onAnswer(true).observe(viewLifecycleOwner, {
                onNewCard(it)
            })
        }
    }

    private fun onNewCard(card: ExamCard?) {
        card?.let {
            currentCard = card
            cardContent.text = currentCard.question
            count++
            counter.text = "$count/$total"
        } ?: run {
            findNavController().navigate(R.id.action_testFragment_to_preTestFragment)
        }
    }

    override fun onButtonClick() {
        dialog.hide()
        findNavController().navigate(R.id.action_testFragment_to_preTestFragment)
    }

}