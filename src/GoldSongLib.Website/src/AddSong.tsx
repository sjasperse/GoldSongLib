import { MouseEvent } from "react";
import { addSong } from "./services/apiClient";
import newGuid from "./services/newGuid";

export default function AddSong() {

  function onCancel(e: MouseEvent<HTMLButtonElement>) {
    e.preventDefault();

    history.back();
  }

  async function onAdd(e: MouseEvent<HTMLButtonElement>) {
    e.preventDefault();
    const nameElement = document.getElementsByName('name')[0] as HTMLInputElement;

    if (!nameElement.value) {
      return;
    }

    await addSong({
      id: newGuid(),
      name: nameElement.value,
      tags: []
    });

    history.back();
  }

  return <>
    <h1>Add Song</h1>
    <form>
      <div style={{ paddingBottom: '2em' }}>
        <label>Name</label>
        <input name="name" type="text" />
      </div>

      <button onClick={onCancel}>Cancel</button>
      <button className="primary" onClick={onAdd}>Add</button>
    </form>
  </>
}