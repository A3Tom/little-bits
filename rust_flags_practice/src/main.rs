
use std::{convert::TryFrom, process, io};
use phf::{phf_map};
use log::{info, warn};

use crate::logger::setup_logger;
mod logger;

static MORSE_MAP: phf::Map<u8, char> = phf_map! {
    04u8 => 'R',
    20u8 => 'E',
    69u8 => 'M',
    07u8 => 'O',
};

fn main() {
    setup_logger(true, None);

    let input_flag = 8;
    let parsed_value;
    match u8::try_from(input_flag).ok() {
        Some(value) => {
            info!("Valid input entered ({:#})", value);
            parsed_value = value
        }
        None => {
            warn!("Yer talkin shite mate, {:#} is outside of acceptable range [{:#}..{:#}]",
                input_flag,
                u8::MIN,
                u8::MAX);
            process::abort();
        }
    };

    match MORSE_MAP.get(&parsed_value) {
        Some(value) => publish_success(value, &input_flag),
        None => warn!("Valid u8 but it's a load a pish, nae mappin for {}", parsed_value)
    }
}

fn publish_success(keyed_char: &char, input_flag: &u8) {
    info!("Yer a fuckin legend, {} maps to {:#} successfully",
        input_flag,
        keyed_char
    )
}