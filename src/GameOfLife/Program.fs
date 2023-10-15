module GameOfLife

open System

let neighbours (x, y) =
  [| (x + 1, y)
     (x, y + 1)
     (x - 1, y)
     (x, y - 1)
     (x + 1, y - 1)
     (x - 1, y + 1)
     (x - 1, y - 1)
     (x + 1, y + 1) |]
  |> Array.distinct

let isAlive grid (x, y) = grid |> Array.contains (x, y)

let aliveNeighbours (x, y) grid = neighbours (x, y) |> Array.filter (isAlive grid) |> Array.length

let survivors grid =
  grid
  |> Array.filter (fun (x, y) ->
    let n = grid |> aliveNeighbours (x, y)
    n = 2 || n = 3)

let births grid =
  grid
  |> Array.collect neighbours
  |> Array.distinct
  |> Array.filter (fun (x, y) -> grid |> aliveNeighbours (x, y) = 3)

let tick grid =
  let next = survivors grid |> Array.append (births grid)
  next |> Array.distinct

let printAt (x, y) (c: char) = Console.Write $"\027[{y + 1};{x + 1}H{c}"

let random width height =
  let r = Random()

  [| 0..width |]
  |> Array.collect (fun x -> [| 0..height |] |> Array.map (fun y -> x, y))
  |> Array.filter (fun _ -> r.Next(0, 5) > 3)

[<EntryPoint>]
let main args =
  let cellChar = '0'
  let delay = TimeSpan.FromMilliseconds 50
  let wrapDelta = 100
  Console.CursorVisible <- false

  let rec loop grid =
    let wrap delta (x, y) =
      0 - delta <= x
      && x < Console.WindowWidth + delta
      && 0 - delta <= y
      && y < Console.WindowHeight + delta

    Console.Clear()
    grid |> Array.filter (wrap wrapDelta) |> Array.iter (fun (x, y) -> cellChar |> printAt (x, y))
    Threading.Thread.Sleep delay
    grid |> tick |> Array.filter (wrap wrapDelta) |> loop

  (Console.WindowWidth, Console.WindowHeight) ||> random |> loop
  0
