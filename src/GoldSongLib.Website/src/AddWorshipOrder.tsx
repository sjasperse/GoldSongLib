import ModifyWorshipOrder from "./WorshipOrderEditor";
import newGuid from "./services/newGuid";
import { WorshipOrder } from "./types";

export default function AddWorshipOrder() {
  const emptyWorshipOrder: WorshipOrder = {
    id: newGuid(),
    date: new Date(),
    songSets: [{
      id: newGuid(),
      title: 'Opening Song',
      songs: [{ id: newGuid(), songId: null }]
    }, {
      id: newGuid(),
      title: 'Song Set',
      songs: [{ id: newGuid(), songId: null }, { id: newGuid(), songId: null }, { id: newGuid(), songId: null }]
    }, {
      id: newGuid(),
      title: 'Song of Response',
      songs: [{ id: newGuid(), songId: null }]
    }],
    tags: []
  };

  return <>
    <h1>Add Worship Order</h1>
    <ModifyWorshipOrder origWorshipOrder={emptyWorshipOrder} />
  </>;
}