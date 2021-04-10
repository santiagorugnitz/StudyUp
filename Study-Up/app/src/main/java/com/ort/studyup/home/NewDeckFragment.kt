package com.ort.studyup.home

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import androidx.lifecycle.observe
import com.ort.studyup.R
import com.ort.studyup.common.DECK_DATA_KEY
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_new_deck.*
import kotlinx.android.synthetic.main.item_spinner.view.*

class NewDeckFragment : BaseFragment() {

    private val viewModel: NewDeckViewModel by injectViewModel(NewDeckViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_new_deck, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        initSpinner(difficultyInput, resources.getStringArray(R.array.difficulties), getString(R.string.difficulty))
        initSpinner(visibilityInput, resources.getStringArray(R.array.visibilities), getString(R.string.visibility))

        (arguments?.getSerializable(DECK_DATA_KEY) as DeckData?)?.let {
            viewModel.deckId = it.id
            header.text = getString(R.string.edit_deck)
            nameInput.setText(it.name)
            subjectInput.setText(it.subject)
            difficultyInput.spinner.setSelection(it.difficulty)
            if (it.isHidden) {
                visibilityInput.spinner.setSelection(1)
            }
            saveButton.text = getString(R.string.save_changes)
        }

        saveButton.setOnClickListener {
            viewModel.sendData(
                DeckData(
                    name = nameInput.text.toString(),
                    subject = subjectInput.text.toString(),
                    difficulty = difficultyInput.spinner.selectedItemPosition,
                    isHidden = visibilityInput.spinner.selectedItemPosition == 1
                )
            ).observe(viewLifecycleOwner) {
                if(it>0){
                    //TODO: navigate to deckDetail using it as id
                }
            }
        }

    }

    private fun initSpinner(input: View, array: Array<String>, title: String) {
        ArrayAdapter(requireActivity(), android.R.layout.simple_spinner_item, array).also {
            it.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
            input.spinner.adapter = it
        }
        input.spinnerTitle.text = title
    }

}