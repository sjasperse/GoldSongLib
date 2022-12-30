import { WorshipOrder } from "../types"
import SongReferenceLibrary from "./SongReferenceLibrary";
import WorshipOrderEditorForm from "./WorshipOrderEditorForm";
import Split from 'react-split'
import { DndProvider } from 'react-dnd'
import { HTML5Backend } from 'react-dnd-html5-backend'


export type WorshipOrderEditorParams = {
  origWorshipOrder: WorshipOrder,
  onSubmit?: (worshipOrder: WorshipOrder) => Promise<void>
}

export default function WorshipOrderEditor(params: WorshipOrderEditorParams) {
  return <>
    <DndProvider backend={HTML5Backend}>
      <Split sizes={[65, 35]} className="split">
          <WorshipOrderEditorForm {...params} />
          <SongReferenceLibrary />
      </Split>
    </DndProvider>
  </>;
}