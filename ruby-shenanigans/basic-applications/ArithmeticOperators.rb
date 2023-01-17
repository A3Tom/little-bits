def add_numbers(x, y)
    return x + y
end

def sum_n_numbers(args)
    return args.sum
end

def subtract_numbers(x, y)
    return x - y
end

def multiply_numbers(x, y)
    return x * y
end

def is_even(x)
    return x.even?
end

def output_check_results()
    puts "4 + 5 = #{add_numbers(4, 5)}"
    sum_numbers = [1, 2, 3, 4]
    puts "Sum of [#{sum_numbers.join(", ")}] = #{sum_n_numbers(sum_numbers)}"
    puts "Is 2 even ? #{is_even(2)}"
    1.upto(5) { |x| puts "#{x} is odd" if !is_even(x)}
end

if __FILE__ == $0
    output_check_results()
end