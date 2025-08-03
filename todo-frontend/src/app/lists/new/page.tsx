'use client';
import { useState, useEffect } from 'react';
import { ListItemService } from '@/services/listItemService';
import { TaskListService } from '@/services/taskListService';
import { TaskList } from '@/types/TaskList';
import { Card, TextInput, HR, Checkbox, Datepicker, Select } from 'flowbite-react';
import { ListItem } from '@/types/ListItem';
import { PriorityLevel } from '@/types/PriorityLevel';

export default function NewTaskList() {
  const taskListService = new TaskListService();
  const listItemService = new ListItemService();

  const [lists, setLists] = useState<TaskList[]>();
  const [listTitle, setListTitle] = useState<string>("");
  const [todos, setTodos] = useState<ListItem[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const res = await taskListService.getAllAsync();
      if (res.errors) {
        console.log(res.errors);
        return;
      }
      setLists(res.data);
    };
    fetchData();
  }, []);

  

  return (
    <Card className="mt-5 w-[48rem] bg-green-50">
      <h1 className="text-2xl font-semibold tracking-tight text-gray-900">
        Creating a new to-do list
      </h1>

      <TextInput
        id="title"
        value={listTitle}
        onChange={(e) => setListTitle(e.target.value)}
        type="text"
        placeholder="Name your list"
        required
      />

      <HR />

      {todos.map((todo, index) => (
        <div key={index} className="grid grid-cols-10 gap-2 mt-4 items-center justify-items-stretch">
          <Checkbox 
            className="col-span-1 justify-self-center"
            checked={todo.isDone}
            onChange={(e) => updateTodo(index, 'isDone', e.target.checked)}
          />
          <TextInput 
            placeholder="What do you need to do?" 
            className="col-span-4 w-full"
            value={todo.description}
            onChange={(e) => updateTodo(index, 'description', e.target.value)}
          />
          <Datepicker 
            label={'Due at'} 
            className="col-span-3 w-full"
            selected={todo.dueAt}
            onSelectedDateChanged={(date) => updateTodo(index, 'dueAt', date)}
          />
          <Select 
            id={`priority-${index}`} 
            className="col-span-1 w-full"
            value={todo.priority}
            onChange={(e) => updateTodo(index, 'priority', e.target.value as PriorityLevel)}
          >
            <option value={PriorityLevel.Low}>Low</option>
            <option value={PriorityLevel.Medium}>Medium</option>
            <option value={PriorityLevel.High}>High</option>
          </Select>
          <button
            type="button"
            onClick={() => removeTodo(index)}
            className="col-span-1 justify-self-center text-red-500 hover:text-red-700"
          >
            ✕
          </button>
        </div>
      ))}

      {/* Add new todo button */}
      <div className="mt-4 flex justify-center">
        <button
          type="button"
          onClick={addTodo}
          className="flex items-center gap-2 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
        >
          <span className="text-lg">+</span>
          Add Todo
        </button>
      </div>

      {/* Optional: Save button for the entire list */}
      <div className="mt-6 flex justify-end">
        <button
          type="button"
          className="px-6 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition-colors"
          onClick={saveList}
        >
          Save List
        </button>
      </div>
    </Card>
  );
}