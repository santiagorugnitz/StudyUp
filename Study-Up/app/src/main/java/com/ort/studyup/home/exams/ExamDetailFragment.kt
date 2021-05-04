package com.ort.studyup.home.exams

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.*
import com.ort.studyup.common.models.Exam
import com.ort.studyup.common.renderers.ExamCardItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_exam_detail.*

class ExamDetailFragment : BaseFragment(), ExamCardItemRenderer.Callback {

    private val adapter = RendererAdapter()
    private val viewModel: ExamDetailViewModel by injectViewModel(ExamDetailViewModel::class)
    private var examId: Int = 0

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_exam_detail, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        examId = arguments?.getInt(DECK_ID_KEY) ?: 0

        adapter.addRenderer(ExamCardItemRenderer(this))
        examCardList.layoutManager = LinearLayoutManager(requireContext())
        examCardList.adapter = adapter
        initViewModel(examId)
    }

    private fun initUI(deck: Exam) {
        title.text = deck.name
        //TODO: change UI if exam is in progress (dont allow to change list)
        addButton.setOnClickListener {
            //TODO
            //findNavController().navigate(R.id.action_deckDetailFragment_to_newFlashcardFragment, Bundle().apply { putInt(DECK_ID_KEY, deck.id) })
        }
    }

    private fun initViewModel(id: Int) {
        viewModel.loadDetails(id).observe(viewLifecycleOwner, { exam ->
            adapter.setItems(exam.examcards.map {
//                FlashcardItemRenderer.Item(
//                    it.id,
//                    it.question,
//                    it.answer
//                )
            })
            initUI(exam)
        }
        )
    }

    override fun onEditExamCard(id: Int, question: String, answer: Boolean) {
        findNavController().navigate(R.id.action_deckDetailFragment_to_newFlashcardFragment,
            Bundle().apply {
                putInt(EXAM_ID_KEY, examId)
                putInt(FLASHCARD_ID_KEY, id)
                putString(QUESTION_KEY, question)
                putBoolean(ANSWER_KEY, answer)
            })
    }

}